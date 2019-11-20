using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HangbankTrainer.Model
{

    public class Training : INotifyPropertyChanged
    {

        private TrainingTypeEnum _trainingType;
        private IntensityTypeEnum _intensityType;

        private double _target;
        private double _bandwidth;

        // Only for Interval type of training. 
        private int _secondsTraining;
        private int _secondsRest; 

        public Training()
        {
            TrainingType = TrainingTypeEnum.Constant;
            IntensityType = IntensityTypeEnum.Mid;

            SecondsTraining = 60;
            SecondsRest = 5; 
            Bandwidth = 2.0;
        }

        public TrainingTypeEnum TrainingType
        {
            get => _trainingType;
            set => SetField(ref _trainingType, value);
        }

        public IntensityTypeEnum IntensityType
        {
            get => _intensityType;
            set => SetField(ref _intensityType, value);
        }

        public double Target
        {
            get => _target;
            set => SetField(ref _target, value);
        }

        public double Bandwidth
        {
            get => _bandwidth;
            set => SetField(ref _bandwidth, value);
        }

        public int SecondsTraining
        {
            get => _secondsTraining;
            set => SetField(ref _secondsTraining, value);
        }

        public int SecondsRest
        {
            get => _secondsRest;
            set => SetField(ref _secondsRest, value);
        }

        public double GenerateTargetAt(double t)
        {
            if (_trainingType == TrainingTypeEnum.Constant)
            {
                return _target;
            }
            else if (_trainingType == TrainingTypeEnum.Interval)
            {
                double period = _secondsTraining + _secondsRest;
                double remainder = t % period;
                if (remainder < _secondsTraining)
                {
                    return _target; 
                }
                else
                {
                    return 0.0; 
                }

            }

            return 0.0;
        }

        public double GenerateTargetMinAt(double t)
        {
            return GenerateTargetAt(t) - _bandwidth;
        }

        public double GenerateTargetMaxAt(double t)
        {
            return GenerateTargetAt(t) + _bandwidth;
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