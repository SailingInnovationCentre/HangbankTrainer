using HangbankTrainer.Communication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HangbankTrainer.Model
{
    public class HangbankModel : INotifyPropertyChanged
    {
        public HangbankModel()
        {
            Listener = new SerialPortListener();
            Training = new Training();

            LinksOnbelast = 394;   // 15 kg op balk van gym. 
            LinksBelast = 748;     // 40 kg op balk van gym. 
            RechtsOnbelast = 188;
            RechtsBelast = 360;
        }

        private SerialPortListener _listener;
        public SerialPortListener Listener
        {
            get => _listener;
            set => SetField(ref _listener, value);
        }

        private Athlete _currentAthlete;
        public Athlete CurrentAthlete
        {
            get => _currentAthlete;
            set => SetField(ref _currentAthlete, value);
        }

        private Training _training;
        public Training Training
        {
            get => _training;
            set => SetField(ref _training, value);
        }

        public ObservableCollection<Athlete> Athletes { get; set; }

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

        public double DetermineMoment(int linksVolt, int rechtsVolt)
        {
            var linksMoment = 100 * (linksVolt - LinksOnbelast) / (LinksBelast - LinksOnbelast);
            var rechtsMoment = 100 * (rechtsVolt - RechtsOnbelast) / (RechtsBelast - RechtsOnbelast);
            return Math.Max(0, Math.Max(linksMoment, rechtsMoment));
        }

        public string DetermineVoltage(int linksVolt, int rechtsVolt)
        {
            var linksMoment = (int)(1000 * (linksVolt - LinksOnbelast) / (LinksBelast - LinksOnbelast));
            var rechtsMoment = (int)(1000 * (rechtsVolt - RechtsOnbelast) / (RechtsBelast - RechtsOnbelast));
            return linksMoment > rechtsMoment ? $"Links: {linksVolt}" : $"Rechts: {rechtsVolt}";
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
