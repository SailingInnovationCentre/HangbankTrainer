using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Timers;

namespace HangbankTrainer
{

    public class SerialPortListener : INotifyPropertyChanged
    {
        private SerialPort _serialPort;
        private string _serialPortName;
        public string SerialPortName
        {
            get => _serialPortName;
            set
            {
                SetField(ref _serialPortName, value);
                SetSerialPort(_serialPortName); 
            }
        }

        private string _cachedString; 

        public event EventHandler NewMessage;

        private Timer _timer;
        private Random _random = new Random(); 

        private void SetSerialPort(string port)
        {
            try
            {
                CloseSerialPort(); 
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

        internal void CloseSerialPort()
        {
            if (_serialPort != null)
            {
                _serialPort.Close();
                _serialPort = null;
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

        private int _currentLeft; 
        private int _currentRight; 

        private void StartTestLoop()
        {
            _currentLeft = 500;
            _currentRight = 240; 

            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Interval = 250;
            _timer.Elapsed += (s, e) =>
            {
                _currentLeft += 1;
                if (_currentLeft > 600)
                {
                    _currentLeft = 500; 
                }

                _currentRight += 10; 
                if (_currentRight > 350)
                {
                    _currentRight = 240; 
                }

                //NewMessage?.Invoke(this, new SerialPortEventArgs(_random.Next(550,560), _random.Next(245, 255)));
                NewMessage?.Invoke(this, new SerialPortEventArgs(_currentLeft, 250));
            };
            _timer.Start(); 
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }
}
