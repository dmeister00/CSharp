using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using rtChart;
using System.IO.Ports;


namespace SerialLogger
{
    

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort serialPort1 = new SerialPort();

        public MainWindow()
        {
            InitializeComponent();

            string[] portNames = SerialPort.GetPortNames();
            foreach (string portName in portNames)
            {
                SerialPort port = new SerialPort(portName);
                SerialPort1Box.Items.Add(portName);
                SerialPort2Box.Items.Add(portName);
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                serialPort1 = new SerialPort(SerialPort1Box.SelectedValue.ToString());

                int baud1;
                if (int.TryParse(Baud1TextBox.Text, out baud1))
                {
                    serialPort1.BaudRate = baud1;
                }

                else
                {
                    MessageBox.Show("Invalid baudrate" + Baud1TextBox.Text, "Error");
                    return;
                }

                serialPort1.Parity = Parity.None;
                serialPort1.StopBits = StopBits.One;
                serialPort1.DataBits = 8;
                serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialDataReceivedEventHandler);
                serialPort1.Open();
                serialPort1.WriteLine("VAL1?");
            }
            //SerialPort serialPort2 = new SerialPort("COM6");
        }

        private void serialDataReceivedEventHandler (object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sData = sender as SerialPort;
            string recvData = sData.ReadLine();

            this.Dispatcher.Invoke(() =>
            {
                SerialData.AppendText(recvData);
                SerialData.ScrollToEnd();
            });
        }
    }
}
