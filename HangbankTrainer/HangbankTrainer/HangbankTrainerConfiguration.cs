using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Windows;

namespace HangbankTrainer
{
    class HangbankTrainerConfiguration : INotifyPropertyChanged
    {

        public HangbankTrainerConfiguration()
        {
            Listener = new SerialPortListener(); 

            _linksOnbelast = 456;   // 100 kg op zitvlak
            _linksBelast = 570;     // 50 kg op 120cm op calibratiebalk. 
            _rechtsOnbelast = 207;
            _rechtsBelast = 256;
        }

        public SerialPortListener Listener { get; private set; }

        public void SetSerialPort(string port)
        {
            Listener.SetSerialPort(port);
        }

        private double _linksOnbelast;
        public double LinksOnbelast
        {
            get => _linksOnbelast;
            set => SetField(ref _linksOnbelast, value);
        }

        private double _linksBelast;
        public double LinksBelast
        {
            get => _linksBelast;
            set => SetField(ref _linksBelast, value);
        }

        private double _rechtsOnbelast;
        public double RechtsOnbelast
        {
            get => _rechtsOnbelast;
            set => SetField(ref _rechtsOnbelast, value);
        }

        private double _rechtsBelast;
        public double RechtsBelast
        {
            get => _rechtsBelast;
            set => SetField(ref _rechtsBelast, value);
        }

        private double _spelerMinimum;
        public double SpelerMinimum
        {
            get => _spelerMinimum;
            set => SetField(ref _spelerMinimum, value);
        }

        private double _spelerMaximum;
        public double SpelerMaximum
        {
            get => _spelerMaximum;
            set => SetField(ref _spelerMaximum, value);
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
