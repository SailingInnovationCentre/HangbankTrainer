﻿using HangbankTrainer.Communication;
using HangbankTrainer.Model;
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

namespace HangbankTrainer.View
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

        private HangbankMainWindow _mainWindow; 
        private HangbankModel _model;

        private DateTime _t0;
        private bool _started = false;

        public TrainingUserControl(HangbankMainWindow mainWindow, HangbankModel model)
        {
            _mainWindow = mainWindow;
            Model = model;

            InitialiseBindings();
            UpdateAxisLimits(0.0); 

            var mapper = Mappers.Xy<MeasureModel>()
                .X(mm => mm.T)
                .Y(mm => mm.Moment);
            Charting.For<MeasureModel>(mapper);

            InitializeComponent();
            DataContext = this;
            CurrentStatus = "Prepare to start...";

            Task.Factory.StartNew(() =>
            {
                // Give athletes 10 seconds before the training starts. 
                Task.Delay(5000).Wait();
                _t0 = DateTime.Now;
                Start();
            });
        }

        private void InitialiseBindings()
        {
            XAxisMin = 0.0;
            XAxisMax = 25.0;
            YAxisMin = 0.0;
            YAxisMax = 200.0;

            MomentValues = new ChartValues<MeasureModel>();
            TargetValues = new ChartValues<MeasureModel>();
            UpperTargetValues = new ChartValues<MeasureModel>();
            LowerTargetValues = new ChartValues<MeasureModel>();

            for (double t=0; t<50; t+=1)
            {
                _model.Training.GenerateTargetAt(t, out double target, out double minTarget, out double maxTarget); 
                TargetValues.Add(new MeasureModel(t, target));
                UpperTargetValues.Add(new MeasureModel(t, maxTarget));
                LowerTargetValues.Add(new MeasureModel(t, minTarget));
            }
        }

        private void StopTrainingButton_Click(object sender, RoutedEventArgs e)
        {
            Stop();
            _mainWindow.StartFrontPage();
        }

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

            var _currentT = (DateTime.Now - _t0).TotalSeconds;
            CurrentStatus = _model.Training.GenerateStatusIndication(_currentT); 

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

            _model.Training.GenerateTargetAt(t + 50.0, out double target, out double minTarget, out double maxTarget);
            TargetValues.Add(new MeasureModel(t + 50.0, target));
            UpperTargetValues.Add(new MeasureModel(t + 50.0, maxTarget)); 
            LowerTargetValues.Add(new MeasureModel(t + 50.0, minTarget)); 

            if (UpperTargetValues.Count > 100)
            {
                TargetValues.RemoveAt(0);
                LowerTargetValues.RemoveAt(0);
                UpperTargetValues.RemoveAt(0);
            }

            UpdateAxisLimits(t);

            _lastGraphUpdate = DateTime.Now; 
        }

        private void UpdateAxisLimits(double t)
        {
            if (t <= 20)
            {
                XAxisMin = 0.0;
                XAxisMax = 25.0;
            }
            else
            {
                XAxisMin = t - 20.0;
                XAxisMax = t + 5.0; 
            }

            // Where-clause here is needed to prevent Y-axis be influenced by NaN values. 
            // If only NaNs (for instance, after an entire training), do not update the YAxis. 
            var lowerTargetValues = LowerTargetValues.Where(mm => !double.IsNaN(mm.Moment));
            if (lowerTargetValues.Any())
            {
                YAxisMin = lowerTargetValues.Min(v => v.Moment) - 10.0;
            }

            var upperTargetValues = UpperTargetValues.Where(mm => !double.IsNaN(mm.Moment));
            if (upperTargetValues.Any())
            {
                YAxisMax = upperTargetValues.Max(v => v.Moment) + 10.0; 
            }
        }

        #region Bindings

        public HangbankModel Model
        {
            get => _model;
            set => SetField(ref _model, value);
        }

        private string _serialInput;
        public string SerialInput
        {
            get => _serialInput;
            set => SetField(ref _serialInput, value);
        }

        private string _currentStatus; 
        public string CurrentStatus
        {
            get => _currentStatus;
            set => SetField(ref _currentStatus, value); 
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

        public ChartValues<MeasureModel> MomentValues { get; set; }
        public ChartValues<MeasureModel> TargetValues { get; set; }
        public ChartValues<MeasureModel> UpperTargetValues { get; set; }
        public ChartValues<MeasureModel> LowerTargetValues { get; set; }

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
            set
            {
                if (Math.Abs(_yAxisMax - value) > 2.0)
                {
                    SetField(ref _yAxisMax, value);
                }
            }
        }

        private double _yAxisMin;
        public double YAxisMin
        {
            get => _yAxisMin;
            set
            {
                if (Math.Abs(_yAxisMin - value) > 2.0)
                {
                    SetField(ref _yAxisMin, value);
                }
            }
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
