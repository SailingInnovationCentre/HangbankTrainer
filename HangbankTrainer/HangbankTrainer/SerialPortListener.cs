using System;
using System.IO.Ports;
using System.Timers;

namespace HangbankTrainer
{

    internal class SerialPortListener
    {
        private SerialPort _serialPort;
        private string _cachedString; 

        public event EventHandler NewMessage;

        private Timer _timer;
        private Random _random = new Random(); 

        internal void SetSerialPort(string port)
        {
            try
            {
                if (_serialPort != null)
                {
                    _serialPort.Close();
                    _serialPort = null;
                }
            }
            catch (Exception)
            {
                // gulp.
            }

            if (_timer != null)
            {
                _timer.Stop();
                _timer = null; 
            }

            if (port == "test")
            {
                StartTestLoop(); 
            }
            else
            {
                _serialPort = new SerialPort(port);

                _serialPort.BaudRate = 9600;
                _serialPort.Parity = Parity.None;
                _serialPort.StopBits = StopBits.One;
                _serialPort.DataBits = 8;
                _serialPort.Handshake = Handshake.None;

                _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

                try
                {
                    _serialPort.Open();
                }
                catch (Exception)
                {
                    // gulp.
                }
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            _cachedString += sp.ReadExisting();

            int indexNewline = _cachedString.IndexOf('\n'); 
            while (indexNewline != -1)
            {
                string newMessage = _cachedString.Substring(0, indexNewline).Trim();
                NewMessage?.Invoke(this, new SerialPortEventArgs(newMessage));
                _cachedString = _cachedString.Substring(indexNewline+1); 
                indexNewline = _cachedString.IndexOf('\n'); 
            }
        }

        private void StartTestLoop()
        {
            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Interval = 500;
            _timer.Elapsed += (s, e) =>
            {
                NewMessage?.Invoke(this, new SerialPortEventArgs(0, _random.Next(245, 255)));
            };
            _timer.Start(); 
        }

    }
}
