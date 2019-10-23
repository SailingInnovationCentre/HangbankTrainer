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
        private int _momentMin;
        private int _momentGame;

        private bool _isChanged; 

        public Athlete(string name, int length, int weight, int momentMin, int momentGame, int momentMax)
        {
            _name = name;
            _lengthCm = length;
            _weightKg = weight;
            _momentMin = momentMin;
            _momentGame = momentGame;
            _momentMax = momentMax;
            
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

        public int MomentMin {
            get => _momentMin;
            set
            {
                if (_momentMin != value)
                {
                    IsChanged = true;
                    SetField(ref _momentMin, value);
                    if (_momentMin >= _momentGame)
                    {
                        MomentGame = _momentMin + 1; 
                    }
                }
            }
        }

        public int MomentGame {
            get => _momentGame;
            set
            {
                if (_momentGame != value)
                {
                    IsChanged = true;
                    SetField(ref _momentGame, value);
                    if (_momentGame >= _momentMax)
                    {
                        MomentMax = _momentGame + 1; 
                    }
                    if (_momentGame <= _momentMin)
                    {
                        MomentMin = _momentGame - 1; 
                    }
                }
            }
        }

        public int MomentMax
        {
            get => _momentMax;
            set
            {
                if (_momentMax != value)
                {
                    IsChanged = true;
                    SetField(ref _momentMax, value);
                    if (_momentMax <= _momentGame)
                    {
                        MomentGame = _momentMax - 1; 
                    }
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
            return new Athlete("Nieuwe atleet", 180, 85, 40, 60, 80)
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
