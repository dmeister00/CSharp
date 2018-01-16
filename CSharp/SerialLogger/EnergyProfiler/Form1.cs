using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using rtChart;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;

namespace EnergyProfiler
{
    public partial class Form1 : Form
    {
        const int measInterval = 500;

        kayChart chartData;
        SerialPort tagPort, flukePort;
        System.Threading.Timer flukeTimer;
        double currentMeas = 0;
        StreamWriter tagStream, flukeStream, statStream;
        Stopwatch upTime, callTimer;
        bool onCall = false, statReady = false;
        int callState = 0, lastState = 0;
        string tagData, flukeData;

        int CallNum, GPSFix, BatLev, ACalls, CCalls, CallStatus;
        char TOS;

        static EventWaitHandle logWaitHandle = new AutoResetEvent(false);
        static EventWaitHandle csvWaitHandle = new AutoResetEvent(false);
        //static EventWaitHandle statWaitHandle = new AutoResetEvent(false);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshComportList();
            //string[] portNames = SerialPort.GetPortNames();
            //foreach (string portName in portNames)
            //{
            //    TagPortBox.Items.Add(portName);
            //    FlukePortBox.Items.Add(portName);
            //}

            //endCallTimer = new Stopwatch();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (StartButton.Text == "Start")
            {
                // reset all variables
                onCall = false;
                callState = 0;
                lastState = 0;
                tagData = "";
                flukeData = "";
                CallNum = 1;
                GPSFix = 0;
                BatLev = 0;
                ACalls = 0;
                CCalls = 0;
                CallStatus = 0;
                TOS = '0';

                upTime = Stopwatch.StartNew();
                //flukeTimer = Stopwatch.StartNew();
                callTimer = new Stopwatch();

                int tagBaud, flukeBaud;

                if ((TagPortBox.SelectedItem != null) && (int.TryParse(TagBaudBox.Text, out tagBaud)) )
                {
                    tagPort = new SerialPort(TagPortBox.SelectedItem.ToString(), tagBaud);
                    tagPort.DataReceived += new SerialDataReceivedEventHandler(TagPortDataReceivedEventHandler);
                    tagPort.ReadTimeout = 5;
                    tagPort.WriteTimeout = 50;
                    try
                    {
                        tagPort.Open();
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageBox.Show("Port in Use", "Error");
                    }
                }

                if ( (FlukePortBox.SelectedItem != null) && (int.TryParse(FlukeBaudBox.Text, out flukeBaud)) )
                {
                    flukePort = new SerialPort(FlukePortBox.SelectedItem.ToString(), flukeBaud);
                    flukePort.DataReceived += new SerialDataReceivedEventHandler(FlukePortDataReceivedEventHandler);
                    flukePort.ReadTimeout = 5;
                    flukePort.WriteTimeout = 50;
                    try
                    {
                        flukePort.Open();
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageBox.Show("Port in Use", "Error");
                    }

                    flukeTimer = new System.Threading.Timer(FlukeTimerCallBack, null, Timeout.Infinite, measInterval);
                    flukeTimer.Change(0, measInterval);
                }

                //flukePort = new SerialPort(FlukePortBox.SelectedItem.ToString(), int.Parse(TagBaudBox.Text));
                //flukePort.DataReceived += new SerialDataReceivedEventHandler(FlukePortDataReceivedEventHandler);
                //flukePort.Open();

                if (ChartEnableCheck.Checked)
                {
                    chartData = new kayChart(FlukeChart, 600);
                    chartData.serieName = "Current";
                }

                

                if (LogEnableCheck.Checked)
                {
                    string path = FileBox.Text;
                    string header = String.Format("\r\n\r\n++++++++++++++++++++++++++++++++++++++\r\n" +
                        "Start Time: {0}\r\n++++++++++++++++++++++++++++++++++++++\r\n", DateTime.Now.ToString());

                    tagStream = File.AppendText(path + ".log");
                    tagStream.WriteLine(header);

                    flukeStream = File.AppendText(path + ".csv");
                    flukeStream.WriteLine(header);
                    flukeStream.WriteLine("Time Current");

                    statStream = File.AppendText(path + ".stat");
                    statStream.WriteLine(header);
                    statStream.WriteLine("Time callNum callTime GPSFix BatLev Attempted Completed TOS CS");

                    // use rx box as to add notes
                    if ((SerialRXBox.TextLength > 0) && (!SerialRXEnableCheck.Checked))
                    {
                        tagStream.WriteLine(SerialRXBox.Text);
                    }

                    tagStream.Flush();
                    flukeStream.Flush();
                    statStream.Flush();

                    Thread logThread = new Thread(WriteToLog);
                    Thread csvThread = new Thread(WriteToCsv);
                    //Thread statThread = new Thread(WriteToStat);
                    logThread.Start();
                    csvThread.Start();
                    //statThread.Start();
                }



                //flukePort.WriteLine("VAL1?");
                StartButton.Text = "Stop";
            }

            else
            {
                if (tagPort != null)
                {
                    tagPort.Close();
                }
                
                if (flukePort != null)
                {
                    flukePort.Close();
                }

                RefreshComportList();
                // refresh COM port list

                tagStream.Close();
                flukeStream.Close();
                statStream.Close();

                flukeTimer.Change(Timeout.Infinite, measInterval);

                StartButton.Text = "Start";
            }
           
        }

        private void RefreshComportList ()
        {
            string[] portNames = SerialPort.GetPortNames();
            TagPortBox.Items.Clear();
            FlukePortBox.Items.Clear();

            foreach (string portName in portNames)
            {
                TagPortBox.Items.Add(portName);
                FlukePortBox.Items.Add(portName);
            }
        }

        // send cmd to get current value
        private void FlukeTimerCallBack(object state)
        {
            try
            {
                flukePort.WriteLine("VAL1?");
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("flukePort is closed", "Error");
            }

            catch (ArgumentNullException)
            {
                MessageBox.Show("flukePort is null", "Error");
            }
        }

        private void TagPortDataReceivedEventHandler(object sender, SerialDataReceivedEventArgs e)
        {
            if ( (tagPort != null) && (tagPort.IsOpen) )
            {
                string recvData = "";

                SerialPort sData = sender as SerialPort;
                try
                {
                    recvData = sData.ReadLine();
                } catch (TimeoutException) { return; }

                //if (recvData.Contains("PowerOnSim"))
                //{
                //    onCall = true;
                //    callTimer.Restart();
                //}

                //else if (recvData.Contains("PowerOffSim"))
                //{
                //    onCall = false;
                //    callTimer.Stop();
                //}

                //else if (recvData.Contains("nextState"))
                //{
                //    int state;
                //    string stateString = recvData.Substring(recvData.IndexOf("nextState") + 9);
                //    // decimal after "nextState" is the state number
                //    if (int.TryParse(stateString, out state))
                //    {
                //        callState = state;
                //    }
                //}


                //if (SerialRXEnableCheck.Checked)
                //{
                //    SerialRXBox.Invoke((MethodInvoker)delegate { SerialRXBox.AppendText(recvData); });
                //}

                //if ( (TimestampCheck.Checked) )
                //{
                //    //tagStream.Write("[{0}] ", DateTime.Now.ToString("hh:mm:ss.fff"));
                //    tagStream.Write("[ {0} ] ", ((double)(upTime.ElapsedMilliseconds)) / 1000);
                //}

                //tagStream.Write(recvData + "\n");
                //tagStream.Flush();

                tagData = recvData + "\n";
                logWaitHandle.Set();
            }
        }

        // save last read
        private void FlukePortDataReceivedEventHandler (object sender, SerialDataReceivedEventArgs e)
        {
            if ((flukePort != null) && (flukePort.IsOpen))
            {
                SerialPort sData = sender as SerialPort;
                string recvData = "";

                try
                {
                    recvData = sData.ReadLine();
                }
                catch (TimeoutException) { return; }

                flukeData = recvData;

                csvWaitHandle.Set();

                //double data;

                //if (Double.TryParse(recvData, out data))
                //{
                //    currentMeas = data;

                //    // false read
                //    if (currentMeas > 2)
                //    {
                //        currentMeas = -1;
                //    }

                //    else if (currentMeas < 0)
                //    {
                //        currentMeas = 0;
                //    }

                //    if (TimestampCheck.Checked)
                //    {
                //        flukeStream.Write("[ {0} ] ", ((double)(upTime.ElapsedMilliseconds)) / 1000);
                //    }

                //    flukeStream.Write(currentMeas);

                //    if (callEnd)
                //    {
                //        if ((endCallTimer.ElapsedMilliseconds) > 10000)
                //        {
                //            callStart = false;
                //            callEnd = false;
                //            callTimer.Stop();
                //            endCallTimer.Stop();
                //        }
                //    }

                //    if (callStart)
                //    {
                //        flukeStream.Write(" ( {0} | {1} )", ((double)(callTimer.ElapsedMilliseconds))/1000, callState);
                //    }

                //    flukeStream.WriteLine();
                //    flukeStream.Flush();

                    //chartData.TriggeredUpdate(currentMeas);

                    // send cmd to get measurement from Fluke multimeter. Need setup first (DCI, Fluke 45)
                    //flukePort.WriteLine("VAL1?");

                    //chartData.TriggeredUpdate(currentMeas);
                //}
            }
            
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            if (tagPort.IsOpen)
            {
                tagPort.Write(SendTextBox.Text);

                if (CRCheck.Checked)
                    tagPort.Write("\r");

                if (LFCheck.Checked)
                    tagPort.Write("\n");
            }
        }

        private void ClearLogButton_Click(object sender, EventArgs e)
        {
            string path = FileBox.Text;
            File.Create(path + ".log").Close();
            File.Create(path + ".csv").Close();
            File.Create(path + ".stat").Close();
        }

        private void FileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.RestoreDirectory = true;
            fileDialog.CheckFileExists = false;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                FileBox.Text = fileDialog.FileName;
            }
        }

        // auto-scroll
        private void SerialRXBox_TextChanged(object sender, EventArgs e)
        {
            if (SerialRXEnableCheck.Checked)
            {
                SerialRXBox.SelectionStart = SerialRXBox.Text.Length;
                SerialRXBox.ScrollToCaret();
            }
        }

        private void WriteToLog()
        {
            do
            {
                if (tagData != "")
                {
                    int tempInt;
                    char tempChar;
                    string intStr, tempStr;

                    if (statReady)
                    {                   
                        /*
                         * Next lines should look like this. For now, assume it's always gonna be 4 lines
                         * 
                         * 0000:   24 36 39 30 32 35 30 43  42 38 36 31 35 31 30 30   $690250CB8615100
                         * 0010:   33 30 36 33 39 39 36 30  32 30 31 38 30 31 31 33   3063996020180113
                         * 0020:   31 32 31 36 31 30 2D 32  30 30 34 30 61 33 34 20   121610 - 20040a34
                         * 0030:   53 54 2D 39 32 30 2D 43  4C 6B 30 30 32 30 33 38   ST-920-CLk002038
                         * 0040:   30 31 33 38 30 30 30 31  30 30 36 31 31 30         01380001006110
                        */

                        if (tagData.Contains("ST-920-CL"))
                        {
                            // skip the version number as well
                            intStr = tagData.Substring(tagData.IndexOf("ST-920-CL") + 13);
                            if (int.TryParse(intStr, out tempInt))
                            {
                                BatLev = tempInt;
                            }
                        }

                        else if (tagData.Contains("0040:"))
                        {
                            int index = 0;
                            // skip the decoded ascii chars columns
                            tempStr = tagData.Substring(tagData.IndexOf("0040:") + 58);
                            tempStr = tempStr.Trim();       // remove white spaces

                            intStr = tempStr.Substring(index, 4);
                            if (int.TryParse(intStr, out tempInt))
                            {
                                ACalls = tempInt;
                            }
                            index += 4;

                            intStr = tempStr.Substring(index, 4);
                            if (int.TryParse(intStr, out tempInt))
                            {
                                CCalls = tempInt;
                            }
                                
                            index += 4 + 2;     // next 2 chars are reserved
                                
                            intStr = tempStr.Substring(index, 1);
                            if (char.TryParse(intStr, out tempChar))
                            {
                                TOS = tempChar;
                            }

                            index += 1;

                            intStr = tempStr.Substring(index, 2);
                            if (int.TryParse(intStr, out tempInt))
                            {
                                CallStatus = tempInt;
                            }

                            WriteToStat();

                            statReady = false;
                            GPSFix = 0;
                            BatLev = 0;
                            ACalls = 0;
                            CCalls = 0;
                            TOS = '0';
                            CallStatus = 0;
                            CallNum++;
                        }
                    }

                    else if (onCall)
                    {
                        // get GPSFix result
                        if (tagData.Contains("fixTime"))
                        {
                            //int fixtime;
                            tempStr = tagData.Substring(tagData.IndexOf("fixTime") + 8);
                            tempStr.Trim();
                            intStr = tempStr.Substring(0, 3);
                            if (int.TryParse(intStr, out tempInt))
                            {
                                GPSFix = tempInt;
                            }
                            //statStream.WriteLine("{0} [{1}] ({2})", GPSFix, intStr, tempStr);
                            //statStream.Flush();
                        }

                        // look for status log
                        else if (tagData.Contains("addEntryToLog"))
                        {
                            statReady = true;
                        }

                        else if (tagData.Contains("PowerOffSim"))
                        {
                            //statWaitHandle.Set();
                            callTimer.Stop();
                            onCall = false;
                            flukeTimer.Change(0, 500);       // reduce sampling rate while not on call
                        }
                    }

                    else if (tagData.Contains("PowerOnSim"))
                    {
                        onCall = true;
                        callTimer.Restart();
                        flukeTimer.Change(0, 50);       // increase sampling rate while on call
                    }

                    //else if (tagData.Contains("nextState"))
                    //{
                    //    int state;
                    //    string stateString = tagData.Substring(tagData.IndexOf("nextState") + 9);
                    //    // decimal after "nextState" is the state number
                    //    if (int.TryParse(stateString, out state))
                    //    {
                    //        callState = state;
                    //    }
                    //}

                    if (SerialRXEnableCheck.Checked)
                    {
                        SerialRXBox.Invoke((MethodInvoker)delegate { SerialRXBox.AppendText(tagData); });
                    }

                    if ((TimestampCheck.Checked))
                    {
                        tagStream.Write("[ {0} ] ", ((double)(upTime.ElapsedMilliseconds)) / 1000);
                    }

                    tagStream.Write(tagData);

                    tagStream.Flush();
                    logWaitHandle.WaitOne();
                }
            }
            while (tagPort.IsOpen);
        }

        private void WriteToCsv()
        {
            do
            {
                if (flukeData != "")
                {
                    if (Double.TryParse(flukeData, out currentMeas))
                    {
                        // false read
                        if (currentMeas > 2)
                        {
                            currentMeas = -1;
                        }

                        else if (currentMeas < 0)
                        {
                            currentMeas = 0;
                        }

                        if (TimestampCheck.Checked)
                        {
                            flukeStream.Write("[ {0} ] ", ((double)(upTime.ElapsedMilliseconds)) / 1000);
                        }

                        flukeStream.Write(currentMeas);

                        //if (onCall)
                        //{
                        //    flukeStream.Write(" {0} {1} ", ((double)(callTimer.ElapsedMilliseconds)) / 1000, callState);
                        //}

                        flukeStream.WriteLine();
                        flukeStream.Flush();

                        if (ChartEnableCheck.Checked)
                        {
                            chartData.TriggeredUpdate(currentMeas);
                        }
                    }

                    flukeData = "";
                    csvWaitHandle.WaitOne();
                }

                //if (flukeTimer.ElapsedMilliseconds >= measInterval)
                //{
                //    flukePort.WriteLine("VAL1?");
                //    flukeTimer.Restart();
                //    csvWaitHandle.WaitOne();
                //}

            } while (flukePort.IsOpen);
        }

        private void WriteToStat()
        {
            // Time callNum callTime GPSFix BatLev Attempted Completed TOS CS
            if ((TimestampCheck.Checked))
            {
                statStream.Write("[ {0} ] ", ((double)(upTime.ElapsedMilliseconds)) / 1000);
            }

            statStream.WriteLine("{0} {1} {2} {3} {4} {5} {6} {7}", CallNum, ((double)(callTimer.ElapsedMilliseconds)) / 1000, GPSFix, BatLev, ACalls, CCalls, TOS, CallStatus);
            statStream.Flush();


            //statWaitHandle.WaitOne();
        }
    }
}
