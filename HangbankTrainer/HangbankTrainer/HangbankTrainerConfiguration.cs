using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HangbankTrainer
{
    class HangbankTrainerConfiguration : INotifyPropertyChanged
    {

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
