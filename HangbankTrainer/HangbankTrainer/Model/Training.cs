using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HangbankTrainer.Model
{

    /// <summary>
    /// Models a 'training'. There are two types of training: 
    /// - Constant: delivers a constant target. 
    /// - Interval: delivers alternately NaN for rest and a constant target: 
    /// 
    ///         TRAINING         REST        TRAINING          REST
    /// <--------------------><------><--------------------><------>
    /// <----------------------------><---------------------------->
    ///        INTERVAL 1                    INTERVAL 2
    ///            
    /// </summary>
    public class Training : INotifyPropertyChanged
    {
        private TrainingTypeEnum _trainingType;
        private IntensityTypeEnum _intensityType;

        private double _target;
        private double _bandwidth;

        // Only applicable to Interval type of training. 
        private int _secondsTraining;
        private int _secondsRest;
        private int _nrOfIntervals; 

        public Training()
        {
            TrainingType = TrainingTypeEnum.Interval;
            IntensityType = IntensityTypeEnum.Mid;

            NrOfIntervals = 12; 
            SecondsTraining = 60;
            SecondsRest = 10; 
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

        public int NrOfIntervals
        {
            get => _nrOfIntervals;
            set => SetField(ref _nrOfIntervals, value);
        }

        public void GenerateTargetAt(double t, out double target, out double min, out double max)
        {
            target = 0.0; 

            if (_trainingType == TrainingTypeEnum.Constant)
            {
                target = _target;
            }
            else if (_trainingType == TrainingTypeEnum.Interval)
            {
                double period = _secondsTraining + _secondsRest;
                if (t > period * _nrOfIntervals)
                {
                    // Training has ended. 
                    target = double.NaN;
                }
                else
                {
                    double remainder = t % period;
                    if (remainder < _secondsTraining)
                    {
                        // Training
                        target = _target;
                    }
                    else
                    {
                        // Switching / resting
                        target = double.NaN;
                    }
                }
            }

            min = target - _bandwidth;
            max = target + _bandwidth; 
        }

        public string GenerateStatusIndication(double t)
        {
            var status = new TrainingStatus(t, this);
            return status.GetStatusText(); 
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