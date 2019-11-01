using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HangbankTrainer
{
    public class Athlete: INotifyPropertyChanged
    {

        private string _name;
        private int _lengthCm;
        private int _weightKg;
        private double _momentMin;
        private double _momentMid;
        private double _momentMax;
        private double _bandwidth; 

        public Athlete(string name, int length, int weight, 
            double momentMin, double momentMid, double momentMax, double bandwidth)
        {
            _name = name;
            _lengthCm = length;
            _weightKg = weight;
            _momentMin = momentMin;
            _momentMid = momentMid;
            _momentMax = momentMax;
            _bandwidth = bandwidth; 
        }

        public string Name {
            get => _name;
            set
            {
                if (_name != value)
                {
                    SetField(ref _name, value);
                }
            }
        }

        public int LengthCm {
            get => _lengthCm;
            set
            {
                if (_lengthCm != value)
                {
                    SetField(ref _lengthCm, value);
                }
            }
        }

        public int WeightKg {
            get => _weightKg;
            set
            {
                if (_weightKg != value)
                {
                    SetField(ref _weightKg, value);
                }
            }
        }

        public double MomentMin {
            get => _momentMin;
            set
            {
                if (_momentMin != value)
                {
                    SetField(ref _momentMin, value);
                    if (_momentMin >= _momentMid - 5)
                    {
                        MomentMid = _momentMin + 5; 
                    }
                }
            }
        }

        public double MomentMid
        {
            get => _momentMid;
            set
            {
                if (_momentMid != value)
                {
                    SetField(ref _momentMid, value);
                    if (_momentMid >= _momentMax - 5)
                    {
                        MomentMax = _momentMid + 5; 
                    }
                    if (_momentMid <= _momentMin + 5)
                    {
                        MomentMin = _momentMid - 5; 
                    }
                }
            }
        }

        public double MomentMax
        {
            get => _momentMax;
            set
            {
                if (_momentMax != value)
                {
                    SetField(ref _momentMax, value);
                    if (_momentMax <= _momentMid + 5)
                    {
                        MomentMid = MomentMax - 5; 
                    }
                }
            }
        }

        public double Bandwidth
        {
            get => _bandwidth;
            set => SetField(ref _bandwidth, value); 
        }

        // This override is necessary for ComboBoxes of CurrentAthlete to work properly. 
        // See: https://stackoverflow.com/questions/19632270/binding-combobox-selecteditem-using-mvvm
        public override string ToString()
        {
            return Name; 
        }

        internal static Athlete CreateNew()
        {
            return new Athlete("Nieuwe atleet", 180, 85, 40, 60, 80, 3.0);
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
