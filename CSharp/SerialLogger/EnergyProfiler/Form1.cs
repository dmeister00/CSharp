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

namespace EnergyProfiler
{
    public partial class Form1 : Form
    {
        kayChart chartData;
        SerialPort tagPort, flukePort;
        System.Threading.Timer flukeTimer;
        double flukeData = 0;
        StreamWriter tagStream;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chartData = new kayChart(FlukeChart, 2400);
            chartData.serieName = "Current";

            string[] portNames = SerialPort.GetPortNames();
            foreach (string portName in portNames)
            {
                TagPortBox.Items.Add(portName);
                FlukePortBox.Items.Add(portName);
            }

            flukeTimer = new System.Threading.Timer(FlukeTimerCallBack, null, Timeout.Infinite, 100);

        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (StartButton.Text == "Start")
            {
                tagPort = new SerialPort(TagPortBox.SelectedItem.ToString(), int.Parse(TagBaudBox.Text));
                tagPort.DataReceived += new SerialDataReceivedEventHandler(TagPortDataReceivedEventHandler);
                tagPort.Open();

                flukePort = new SerialPort(FlukePortBox.SelectedItem.ToString(), int.Parse(TagBaudBox.Text));
                flukePort.DataReceived += new SerialDataReceivedEventHandler(FlukePortDataReceivedEventHandler);
                flukePort.Open();

                flukeTimer.Change(0, 100);

                string path = FileBox.Text;
                tagStream = File.AppendText(path);
                tagStream.WriteLine("\r\n\r\n++++++++++++++++++++++++++++++++++++++\r\n" +
                    "Start Time: {0}\r\n++++++++++++++++++++++++++++++++++++++\r\n", DateTime.Now.ToString());
                tagStream.Flush();
                

                StartButton.Text = "Stop";
            }

            else
            {
                tagPort.Close();
                flukePort.Close();
                tagStream.Close();

                flukeTimer.Change(Timeout.Infinite, 100);

                StartButton.Text = "Start";
            }
           
        }

        // send cmd to get current value
        private void FlukeTimerCallBack(object state)
        {
            // update last value, get next value
            chartData.TriggeredUpdate(flukeData);

            if (flukePort.IsOpen)
            {
                // send cmd to get measurement from Fluke multimeter. Need setup first (DCI, Fluke 45)
                flukePort.WriteLine("VAL1?");
            }
        }

        private void TagPortDataReceivedEventHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sData = sender as SerialPort;
            string recvData = sData.ReadLine();
            SerialRXBox.Invoke((MethodInvoker)delegate { SerialRXBox.AppendText(recvData); });
            tagStream.WriteLine("[{0}] {1}", DateTime.Now.ToString("hh:mm:ss.fff"), recvData);
            tagStream.Flush();
        }

        // save last read
        private void FlukePortDataReceivedEventHandler (object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sData = sender as SerialPort;
            string recvData = sData.ReadLine();
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

                //chartData.TriggeredUpdate(flukeData);
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            tagPort.Write(SendTextBox.Text);

            if (CRCheck.Checked)
                tagPort.Write("\r");

            if (LFCheck.Checked)
                tagPort.Write("\n");
        }

        private void FileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                FileBox.Text = fileDialog.FileName;
            }
        }

        // auto-scroll
        private void SerialRXBox_TextChanged(object sender, EventArgs e)
        {
            SerialRXBox.SelectionStart = SerialRXBox.Text.Length;
            SerialRXBox.ScrollToCaret();
        }

    }
}
