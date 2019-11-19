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

        public Training()
        {
            Bandwidth = 2.0;
            TrainingType = TrainingTypeEnum.Constant;
            IntensityType = IntensityTypeEnum.Mid;
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

        public double GenerateTargetAt(double t)
        {
            return _target;
        }

        public double GenerateTargetMinAt(double t)
        {
            return _target - _bandwidth;
        }

        public double GenerateTargetMaxAt(double t)
        {
            return _target + _bandwidth;
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