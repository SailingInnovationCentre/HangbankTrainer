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
        private int _momentMax;
        private int _moment75;
        private int _moment145Degrees;

        private bool _isChanged; 

        public Athlete(string name, int length, int weight, int momentMax, int moment75, int moment145degrees)
        {
            _name = name;
            _lengthCm = length;
            _weightKg = weight;
            _momentMax = momentMax;
            _moment75 = moment75;
            _moment145Degrees = moment145degrees;

            _isChanged = false;
        }

        public string Name {
            get => _name;
            set
            {
                if (_name != value)
                {
                    IsChanged = true; 
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
                    IsChanged = true;
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
                    IsChanged = true;
                    SetField(ref _weightKg, value);
                }
            }
        }

        public int MomentMax {
            get => _momentMax;
            set
            {
                if (_momentMax != value)
                {
                    IsChanged = true;
                    SetField(ref _momentMax, value);
                }
            }
        }

        public int Moment75 {
            get => _moment75;
            set
            {
                if (_moment75 != value)
                {
                    IsChanged = true;
                    SetField(ref _moment75, value);
                }
            }
        }

        public int Moment145Degrees {
            get => _moment145Degrees;
            set
            {
                if (_moment145Degrees != value)
                {
                    IsChanged = true;
                    SetField(ref _moment145Degrees, value);
                }
            }
        }

        public bool IsChanged {
            get => _isChanged;
            set => SetField(ref _isChanged, value); 
        }

        public bool IsNew { 
            get; 
            set; 
        }

        internal static Athlete CreateNew()
        {
            return new Athlete("Nieuwe atleet", 180, 85, 0, 0, 0)
            {
                IsNew = true
            };
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
