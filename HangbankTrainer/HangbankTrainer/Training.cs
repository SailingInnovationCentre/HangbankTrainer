using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HangbankTrainer
{

    public class Training: INotifyPropertyChanged
    {

        private double _bandwidth;
        private TrainingTypeEnum _trainingType;
        private IntensityTypeEnum _intensityType; 

        public Training()
        {
            Bandwidth = 2.0;
            TrainingType = TrainingTypeEnum.Interval;
            IntensityType = IntensityTypeEnum.Middel;
        }

        public double Bandwidth
        {
            get => _bandwidth;
            set => SetField(ref _bandwidth, value);
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