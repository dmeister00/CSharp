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
        const int measInterval = 100;

        kayChart chartData;
        SerialPort tagPort, flukePort;
        System.Threading.Timer flukeTimer;
        double flukeData = 0;
        StreamWriter tagStream, flukeStream;
        Stopwatch upTime, callTimer, endCallTimer;
        bool callStart = false, callEnd = false;
        int callState = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] portNames = SerialPort.GetPortNames();
            foreach (string portName in portNames)
            {
                TagPortBox.Items.Add(portName);
                FlukePortBox.Items.Add(portName);
            }

            flukeTimer = new System.Threading.Timer(FlukeTimerCallBack, null, Timeout.Infinite, measInterval);
            upTime = new Stopwatch();
            callTimer = new Stopwatch();
            endCallTimer = new Stopwatch();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (StartButton.Text == "Start")
            {
                upTime.Restart();
                int tagBaud, flukeBaud;

                if ((TagPortBox.SelectedItem != null) && (int.TryParse(TagBaudBox.Text, out tagBaud)) )
                {
                    tagPort = new SerialPort(TagPortBox.SelectedItem.ToString(), tagBaud);
                    tagPort.DataReceived += new SerialDataReceivedEventHandler(TagPortDataReceivedEventHandler);
                    tagPort.ReadTimeout = 10;
                    tagPort.WriteTimeout = 100;
                    tagPort.Open();
                }

                if ( (FlukePortBox.SelectedItem != null) && (int.TryParse(FlukeBaudBox.Text, out flukeBaud)) )
                {
                    flukePort = new SerialPort(FlukePortBox.SelectedItem.ToString(), flukeBaud);
                    flukePort.DataReceived += new SerialDataReceivedEventHandler(FlukePortDataReceivedEventHandler);
                    flukePort.ReadTimeout = 10;
                    flukePort.WriteTimeout = 100;
                    flukePort.Open();

                }

                //flukePort = new SerialPort(FlukePortBox.SelectedItem.ToString(), int.Parse(TagBaudBox.Text));
                //flukePort.DataReceived += new SerialDataReceivedEventHandler(FlukePortDataReceivedEventHandler);
                //flukePort.Open();

                if (ChartEnableCheck.Checked)
                {
                    chartData = new kayChart(FlukeChart, 600);
                    chartData.serieName = "Current";
                }

                flukeTimer.Change(0, measInterval);

                if (LogEnableCheck.Checked)
                {
                    string path = FileBox.Text;
                    tagStream = File.AppendText(path + ".log");
                    tagStream.WriteLine("\r\n\r\n++++++++++++++++++++++++++++++++++++++\r\n" +
                        "Start Time: {0}\r\n++++++++++++++++++++++++++++++++++++++\r\n", DateTime.Now.ToString());

                    flukeStream = File.AppendText(path + ".csv");
                    flukeStream.WriteLine("\r\n\r\n++++++++++++++++++++++++++++++++++++++\r\n" +
                        "Start Time: {0}\r\n++++++++++++++++++++++++++++++++++++++\r\n", DateTime.Now.ToString());

                    // use rx box as to add notes
                    if ((SerialRXBox.TextLength > 0) && (!SerialRXEnableCheck.Checked))
                    {
                        tagStream.WriteLine(SerialRXBox.Text);
                        flukeStream.WriteLine(SerialRXBox.Text);
                    }

                    tagStream.Flush();
                    flukeStream.Flush();
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

                // refresh COM port list
                string[] portNames = SerialPort.GetPortNames();
                TagPortBox.Items.Clear();
                FlukePortBox.Items.Clear();

                foreach (string portName in portNames)
                {
                    TagPortBox.Items.Add(portName);
                    FlukePortBox.Items.Add(portName);
                }

                tagStream.Close();
                flukeStream.Close();

                flukeTimer.Change(Timeout.Infinite, measInterval);

                StartButton.Text = "Start";
            }
           
        }

        // send cmd to get current value
        private void FlukeTimerCallBack(object state)
        {
            //// update last value, get next value
            if (ChartEnableCheck.Checked)
            {
                chartData.TriggeredUpdate(flukeData);
            }
            
            //if (TimestampCheck.Checked)
            //{
            //    flukeStream.Write("[{0}] ", DateTime.Now.ToString("hh:mm:ss.fff"));
            //}

            //flukeStream.WriteLine(flukeData);
            //flukeStream.Flush();

            if ( (flukePort != null) && (flukePort.IsOpen) )
            {
                // send cmd to get measurement from Fluke multimeter. Need setup first (DCI, Fluke 45)
                flukePort.WriteLine("VAL1?");
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

                if (recvData.Contains("startNextCall"))
                {
                    callStart = true;
                    callTimer.Restart();
                }

                else if (recvData.Contains("SIM-POWER OFF"))
                {
                    callEnd = true;
                    endCallTimer.Restart();
                    callState = 0;
                }

                else if (recvData.Contains("nextState"))
                {
                    int state;
                    string stateString = recvData.Substring(recvData.IndexOf("nextState") + 9);
                    if (int.TryParse(stateString, out state))
                    {
                        callState = state;
                    }
                }


                if (SerialRXEnableCheck.Checked)
                {
                    SerialRXBox.Invoke((MethodInvoker)delegate { SerialRXBox.AppendText(recvData); });
                }

                if ( (TimestampCheck.Checked) )
                {
                    //tagStream.Write("[{0}] ", DateTime.Now.ToString("hh:mm:ss.fff"));
                    tagStream.Write("[ {0} ] ", ((double)(upTime.ElapsedMilliseconds)) / 1000);
                }

                tagStream.Write(recvData + "\n");
                tagStream.Flush();
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

                double data;

                if (Double.TryParse(recvData, out data))
                {
                    flukeData = data;

                    // false read
                    if (flukeData > 2)
                    {
                        flukeData = -1;
                    }

                    else if (flukeData < 0)
                    {
                        flukeData = 0;
                    }

                    if (TimestampCheck.Checked)
                    {
                        flukeStream.Write("[ {0} ] ", ((double)(upTime.ElapsedMilliseconds)) / 1000);
                    }

                    flukeStream.Write(flukeData);

                    if (callEnd)
                    {
                        if ((endCallTimer.ElapsedMilliseconds) > 10000)
                        {
                            callStart = false;
                            callEnd = false;
                            callTimer.Stop();
                            endCallTimer.Stop();
                        }
                    }

                    if (callStart)
                    {
                        flukeStream.Write(" ( {0} | {1} )", ((double)(callTimer.ElapsedMilliseconds))/1000, callState);
                    }

                    flukeStream.WriteLine();
                    flukeStream.Flush();

                    //chartData.TriggeredUpdate(flukeData);

                    // send cmd to get measurement from Fluke multimeter. Need setup first (DCI, Fluke 45)
                    //flukePort.WriteLine("VAL1?");

                    //chartData.TriggeredUpdate(flukeData);
                }
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

        //private void WriteToTagLog()
        //{

        //}

        //private void GetCurrentMeas()
        //{
        //    flukePort.WriteLine("VAL1?");
        //    if (hasFlukeData)
        //    {

        //    }
        //}
    }
}
