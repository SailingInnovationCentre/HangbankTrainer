using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Timers;

namespace HangbankTrainer.Communication
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
                _cachedString = _cachedString.Substring(indexNewline + 1);
                indexNewline = _cachedString.IndexOf('\n');
            }
        }

        private int _counter;

        private void StartTestLoop()
        {
            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Interval = 100;
            _timer.Elapsed += (s, e) =>
            {
                _counter += 1;
                NewMessage?.Invoke(this, new SerialPortEventArgs((int)(700 + 20 * Math.Sin(_counter / 30.0)), 188));
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
