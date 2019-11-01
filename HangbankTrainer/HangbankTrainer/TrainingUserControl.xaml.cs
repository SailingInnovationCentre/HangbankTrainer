using LiveCharts;
using LiveCharts.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
            public double T;
            public double Moment;

            public MeasureModel(double t, double moment)
            {
                T = t;
                Moment = moment; 
            }
        }

        private DateTime _t0; 

        public ChartValues<MeasureModel> MomentValues { get; set; }
        public ChartValues<MeasureModel> UpperTargetValues { get; set; }
        public ChartValues<MeasureModel> LowerTargetValues { get; set; }

        private HangbankMainWindow _mainWindow; 
        private HangbankModel _model;
        public HangbankModel Model
        {
            get => _model;
            set => SetField(ref _model, value);
        }

        public TrainingUserControl(HangbankMainWindow mainWindow, HangbankModel model)
        {
            _mainWindow = mainWindow;
            Model = model;

            InitialiseBindings();
            UpdateAxisLimits(); 

            var mapper = Mappers.Xy<MeasureModel>()
                .X(mm => mm.T)
                .Y(mm => mm.Moment);
            Charting.For<MeasureModel>(mapper);

            InitializeComponent();
            DataContext = this;

            Task.Factory.StartNew(() =>
            {
                // Give athletes 10 seconds before the training starts. 
                //Task.Delay(0).Wait();
                Task.Delay(10000).Wait();
                _t0 = DateTime.Now;
                Start();
            });
        }

        private void InitialiseBindings()
        {
            XAxisMin = 0.0;
            XAxisMax = 25.0;
            YAxisMin = 30.0;
            YAxisMax = 70.0;

            MomentValues = new ChartValues<MeasureModel>();
            UpperTargetValues = new ChartValues<MeasureModel>();
            LowerTargetValues = new ChartValues<MeasureModel>();

            for (double t=0; t<50; t+=1)
            {
                UpperTargetValues.Add(new MeasureModel(t, _model.Training.GenerateTargetMaxAt(t)));
                LowerTargetValues.Add(new MeasureModel(t, _model.Training.GenerateTargetMinAt(t)));
            }
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

        private DateTime _lastSmallUpdate;
        private DateTime _lastGraphUpdate;

        private void OnMessage(object sender, EventArgs e)
        {
            SmallUpdate(e);
            GraphUpdate(e);
        }

        private void SmallUpdate(EventArgs e)
        {
            if (_lastSmallUpdate != null && DateTime.Now - _lastSmallUpdate < TimeSpan.FromMilliseconds(200))
            {
                return;
            }

            CurrentTimeSpan = DateTime.Now - _t0;

            var eventArgs = (SerialPortEventArgs)e;
            var moment = _model.DetermineMoment(eventArgs.Left, eventArgs.Right);
            var voltage = _model.DetermineVoltage(eventArgs.Left, eventArgs.Right);

            CurrentMoment = moment;
            CurrentVolt = voltage;

            _lastSmallUpdate = DateTime.Now;
        }

        private void GraphUpdate(EventArgs e)
        {
            if (_lastGraphUpdate != null && DateTime.Now - _lastGraphUpdate < TimeSpan.FromSeconds(1))
            {
                return;
            }

            var eventArgs = (SerialPortEventArgs)e;
            var moment = _model.DetermineMoment(eventArgs.Left, eventArgs.Right);

            var t = (DateTime.Now - _t0).TotalSeconds;

            MomentValues.Add(new MeasureModel(t, moment)); 
            if (MomentValues.Count > 100)
            {
                MomentValues.RemoveAt(0);
            }

            UpperTargetValues.Add(new MeasureModel(t + 50.0, _model.Training.GenerateTargetMaxAt(t + 50.0))); 
            LowerTargetValues.Add(new MeasureModel(t + 50.0, _model.Training.GenerateTargetMinAt(t + 50.0))); 
            if (UpperTargetValues.Count > 100)
            {
                LowerTargetValues.RemoveAt(0);
                UpperTargetValues.RemoveAt(0);
            }

            UpdateAxisLimits();

            _lastGraphUpdate = DateTime.Now; 
        }

        private void UpdateAxisLimits()
        {
            if (CurrentTimeSpan.TotalSeconds <= 20)
            {
                XAxisMin = 0.0;
                XAxisMax = 25.0;
            }
            else
            {
                XAxisMin = (CurrentTimeSpan - TimeSpan.FromSeconds(20)).TotalSeconds;
                XAxisMax = (CurrentTimeSpan + TimeSpan.FromSeconds(5)).TotalSeconds;
            }

            YAxisMin = LowerTargetValues.Min(v => v.Moment) - 20.0;
            YAxisMax = UpperTargetValues.Max(v => v.Moment) + 20.0;
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

        private double _xAxisMax;
        public double XAxisMax
        {
            get => _xAxisMax;
            set => SetField(ref _xAxisMax, value);
        }

        private double _xAxisMin;
        public double XAxisMin
        {
            get => _xAxisMin;
            set => SetField(ref _xAxisMin, value);
        }

        private double _yAxisMax;
        public double YAxisMax
        {
            get => _yAxisMax;
            set => SetField(ref _yAxisMax, value);
        }

        private double _yAxisMin;
        public double YAxisMin
        {
            get => _yAxisMin;
            set => SetField(ref _yAxisMin, value);
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
