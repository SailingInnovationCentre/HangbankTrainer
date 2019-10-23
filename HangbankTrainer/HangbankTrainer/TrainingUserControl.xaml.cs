using LiveCharts;
using LiveCharts.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HangbankTrainer
{
    /// <summary>
    /// Interaction logic for TrainingUserControl.xaml
    /// </summary>
    public partial class TrainingUserControl : UserControl, INotifyPropertyChanged
    {
        public class MeasureModel
        {
            public TimeSpan X;
            public double Y;
        }

        private DateTime _t0; 

        public ChartValues<MeasureModel> MomentValues { get; set; }
        public ChartValues<MeasureModel> MaxUpperValues { get; set; }
        public ChartValues<MeasureModel> MaxLowerValues { get; set; }
        public ChartValues<MeasureModel> MidUpperValues { get; set; }
        public ChartValues<MeasureModel> MidLowerValues { get; set; }
        public ChartValues<MeasureModel> MinUpperValues { get; set; }
        public ChartValues<MeasureModel> MinLowerValues { get; set; }

        private HangbankMainWindow _mainWindow; 
        private HangbankModel _model;
        private Training _training; 

        public TrainingUserControl(HangbankMainWindow mainWindow, HangbankModel model, Training training)
        {
            _mainWindow = mainWindow;
            _model = model;
            _training = training;

            AxisMin = 0.0;
            AxisMax = 25.0; 

            InitializeComponent();

            var mapper = Mappers.Xy<MeasureModel>()
                .X(mm => mm.X.TotalSeconds)
                .Y(mm => mm.Y);
            Charting.For<MeasureModel>(mapper);

            MomentValues = new ChartValues<MeasureModel>();
            SetBoundaries(); 
            DataContext = this;

            Task.Factory.StartNew(() =>
            {
                Task.Delay(10000).Wait();
                _t0 = DateTime.Now;
                Start();
            });
        }

        private void SetBoundaries()
        {
            double maxUpper = _model.CurrentAthlete.MomentMax + _training.Bandwidth;
            double maxLower = _model.CurrentAthlete.MomentMax - _training.Bandwidth;

            double midUpper = _model.CurrentAthlete.MomentGame + _training.Bandwidth;
            double midLower = _model.CurrentAthlete.MomentGame - _training.Bandwidth;

            double minUpper = _model.CurrentAthlete.MomentMin + _training.Bandwidth;
            double minLower = _model.CurrentAthlete.MomentMin - _training.Bandwidth;

            TimeSpan ts0 = TimeSpan.Zero;
            TimeSpan ts1 = TimeSpan.FromMinutes(60);  

            MaxUpperValues = new ChartValues<MeasureModel>();
            MaxUpperValues.Add(new MeasureModel { X = ts0, Y = maxUpper });
            MaxUpperValues.Add(new MeasureModel { X = ts1, Y = maxUpper });

            MaxLowerValues = new ChartValues<MeasureModel>();
            MaxLowerValues.Add(new MeasureModel { X = ts0, Y = maxLower });
            MaxLowerValues.Add(new MeasureModel { X = ts1, Y = maxLower });

            MidUpperValues = new ChartValues<MeasureModel>();
            MidUpperValues.Add(new MeasureModel { X = ts0, Y = midUpper });
            MidUpperValues.Add(new MeasureModel { X = ts1, Y = midUpper });

            MidLowerValues = new ChartValues<MeasureModel>();
            MidLowerValues.Add(new MeasureModel { X = ts0, Y = midLower });
            MidLowerValues.Add(new MeasureModel { X = ts1, Y = midLower });

            MinUpperValues = new ChartValues<MeasureModel>();
            MinUpperValues.Add(new MeasureModel { X = ts0, Y = minUpper });
            MinUpperValues.Add(new MeasureModel { X = ts1, Y = minUpper });

            MinLowerValues = new ChartValues<MeasureModel>();
            MinLowerValues.Add(new MeasureModel { X = ts0, Y = minLower });
            MinLowerValues.Add(new MeasureModel { X = ts1, Y = minLower });

            TrainingCartesianChart.AxisY[0].MinValue = ((int)((minLower - 25) / 10)) * 10;
            TrainingCartesianChart.AxisY[0].MaxValue = ((int)((maxUpper + 25) / 10)) * 10;
        }

        private void StopTrainingButton_Click(object sender, RoutedEventArgs e)
        {
            Stop();
            _mainWindow.StartFrontPage();
        }

        private bool _started = false;

        private void Start()
        {
            if (!_started)
            {
                _started = true;
                _model.Listener.NewMessage += OnMessage;
            }
        }

        private void Stop()
        {
            if (_started)
            {
                _started = false;
                _model.Listener.NewMessage -= OnMessage;
            }
        }

        private void OnMessage(object sender, EventArgs e)
        {
            CurrentTimeSpan = DateTime.Now - _t0; 

            var eventArgs = (SerialPortEventArgs)e;
            var moment = _model.DetermineMoment(eventArgs.Left, eventArgs.Right);
            var voltage = _model.DetermineVoltage(eventArgs.Left, eventArgs.Right);

            CurrentMoment = moment;
            CurrentVolt = voltage; 

            MomentValues.Add(new MeasureModel
            {
                X = CurrentTimeSpan,
                Y = moment
            });

            SetAxisLimits();

            if (MomentValues.Count > 1000)
            {
                MomentValues.RemoveAt(0);
            }
        }

        private double _axisMax;
        public double AxisMax
        {
            get => _axisMax;
            set => SetField(ref _axisMax, value);
        }

        private double _axisMin;
        public double AxisMin
        {
            get => _axisMin;
            set => SetField(ref _axisMin, value);
        }

        private void SetAxisLimits()
        {
            if (CurrentTimeSpan.TotalSeconds <= 20)
            {
                AxisMin = 0.0;
                AxisMax = 25.0;
            }
            else
            {
                AxisMin = (CurrentTimeSpan - TimeSpan.FromSeconds(20)).TotalSeconds;
                AxisMax = (CurrentTimeSpan + TimeSpan.FromSeconds(5)).TotalSeconds;
            }
        }

        #region Bindings

        private string _serialInput;
        public string SerialInput
        {
            get => _serialInput;
            set => SetField(ref _serialInput, value);
        }

        private TimeSpan _currentTimeSpan; 
        public TimeSpan CurrentTimeSpan
        {
            get => _currentTimeSpan;
            set => SetField(ref _currentTimeSpan, value); 
        }

        private string _currentVolt;
        public string CurrentVolt
        {
            get => _currentVolt;
            set => SetField(ref _currentVolt, value);
        }

        private double _currentMoment;
        public double CurrentMoment
        {
            get => _currentMoment;
            set => SetField(ref _currentMoment, value);
        }

        #endregion

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
