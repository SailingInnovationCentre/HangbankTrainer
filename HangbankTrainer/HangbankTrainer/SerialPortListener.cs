using System;
using System.IO.Ports;

namespace HangbankTrainer
{

    internal class SerialPortListener
    {
        private SerialPort _serialPort;
        private string _cachedString; 

        public event EventHandler NewMessage;

        internal void SetSerialPort(string port)
        {
            try
            {
                if (_serialPort != null)
                {
                    _serialPort.Open();
                }
            }
            catch (Exception)
            {
                // gulp.
            }

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

    }
}
