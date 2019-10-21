using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HangbankTrainer
{
    public class HangbankModel : INotifyPropertyChanged
    {

        public HangbankModel()
        {
            Listener = new SerialPortListener();

            _linksOnbelast = 456;   // 100 kg op zitvlak
            _linksBelast = 570;     // 50 kg op 120cm op calibratiebalk. 
            _rechtsOnbelast = 207;
            _rechtsBelast = 256;
        }

        private Athlete _currentAthlete;

        public Athlete CurrentAthlete
        {
            get => _currentAthlete;
            set => SetField(ref _currentAthlete, value);
        }

        public ObservableCollection<Athlete> Athletes { get; set; }

        internal SerialPortListener Listener { get; private set; }

        internal void SetSerialPort(string port)
        {
            Listener.SetSerialPort(port);
        }

        private double _linksOnbelast;
        internal double LinksOnbelast
        {
            get => _linksOnbelast;
            set => SetField(ref _linksOnbelast, value);
        }

        private double _linksBelast;
        internal double LinksBelast
        {
            get => _linksBelast;
            set => SetField(ref _linksBelast, value);
        }

        private double _rechtsOnbelast;
        internal double RechtsOnbelast
        {
            get => _rechtsOnbelast;
            set => SetField(ref _rechtsOnbelast, value);
        }

        private double _rechtsBelast;
        internal double RechtsBelast
        {
            get => _rechtsBelast;
            set => SetField(ref _rechtsBelast, value);
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
