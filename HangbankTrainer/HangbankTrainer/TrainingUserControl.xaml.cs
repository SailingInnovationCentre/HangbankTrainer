using LiveCharts;
using LiveCharts.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
            public int X;
            public int Y;
        }

        private int _currentX;

        public ChartValues<MeasureModel> ChartValuesLeft { get; set; }
        public ChartValues<MeasureModel> ChartValuesRight { get; set; }

        private HangbankMainWindow _mainWindow; 
        private HangbankModel _model;

        public TrainingUserControl(HangbankMainWindow mainWindow, HangbankModel model)
        {
            _mainWindow = mainWindow;
            _model = model; 

            AxisMin = 0;
            AxisMax = 100;

            InitializeComponent();

            var mapper = Mappers.Xy<MeasureModel>()
                .X(model => model.X)
                .Y(model => model.Y);

            Charting.For<MeasureModel>(mapper);

            ChartValuesLeft = new ChartValues<MeasureModel>();
            ChartValuesRight = new ChartValues<MeasureModel>();
            _currentX = 0;

            DataContext = this;

            Start();  // TODO: Stop! 
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
                _model.Listener.NewMessage -= OnMessage;
            }
        }

        private void OnMessage(object sender, EventArgs e)
        {
            
            App.Current.Dispatcher.Invoke(() => {
                var eventArgs = (SerialPortEventArgs)e;

                LinksVolt = eventArgs.Left;
                LinksMoment = (int)(1000 * (LinksVolt - _model.LinksOnbelast) / (_model.LinksBelast - _model.LinksOnbelast));

                ChartValuesLeft.Add(new MeasureModel
                {
                    X = _currentX,
                    Y = LinksMoment
                });

                RechtsVolt = eventArgs.Right;
                RechtsMoment = (int)(1000 * (RechtsVolt - _model.RechtsOnbelast) / (_model.RechtsBelast - _model.RechtsOnbelast));

                ChartValuesRight.Add(new MeasureModel
                {
                    X = _currentX,
                    Y = RechtsMoment
                });

                SetAxisLimits(_currentX);
                _currentX++;

                if (ChartValuesLeft.Count > 1000)
                {
                    ChartValuesLeft.RemoveAt(0);
                    ChartValuesRight.RemoveAt(0);
                }
            });
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

        private void SetAxisLimits(int currentX)
        {
            AxisMax = currentX + 1;
            AxisMin = currentX - 60;
        }

        #region Bindings

        private string _serialInput;
        public string SerialInput
        {
            get => _serialInput;
            set => SetField(ref _serialInput, value);
        }

        private int _linksVolt;
        public int LinksVolt
        {
            get => _linksVolt;
            set => SetField(ref _linksVolt, value);
        }

        private int _linksMmoment;
        public int LinksMoment
        {
            get => _linksMmoment;
            set => SetField(ref _linksMmoment, value);
        }

        private double _linksPercentage;
        public double LinksPercentage
        {
            get => _linksPercentage;
            set => SetField(ref _linksPercentage, value);
        }

        private int _rechtsVolt;
        public int RechtsVolt
        {
            get => _rechtsVolt;
            set => SetField(ref _rechtsVolt, value);
        }

        private int _rechtsMmoment;
        public int RechtsMoment
        {
            get => _rechtsMmoment;
            set => SetField(ref _rechtsMmoment, value);
        }

        private double _rechtsPercentage;
        public double RechtsPercentage
        {
            get => _rechtsPercentage;
            set => SetField(ref _rechtsPercentage, value);
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
