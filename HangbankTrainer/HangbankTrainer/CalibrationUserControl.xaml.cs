using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace HangbankTrainer
{
    /// <summary>
    /// Interaction logic for CalibrationUserControl.xaml
    /// </summary>
    public partial class CalibrationUserControl : UserControl, INotifyPropertyChanged
    {

        private HangbankMainWindow _mainWindow;

        private HangbankModel _model;
        public HangbankModel Model
        {
            get => _model;
            set => SetField(ref _model, value); 
        }

        public CalibrationUserControl(HangbankMainWindow mainWindow, HangbankModel model)
        {
            _mainWindow = mainWindow;
            _model = model;
            _model.Listener.NewMessage += OnMessage;

            InitializeComponent();

            DataContext = this;

            VoltValue = "0";
        }

        private string _voltValue; 
        public string VoltValue
        {
            get => _voltValue;
            set => SetField(ref _voltValue, value); 
        }

        private double _momentValue;
        public double MomentValue
        {
            get => _momentValue;
            set => SetField(ref _momentValue, value);
        }

        private void OnMessage(object sender, EventArgs e)
        {
            MomentValue = CalculateMoment(e); 
            VoltValue = CalculateVoltage(e); 
        }

        private int _secondsLeft; 
        public int SecondsLeft
        {
            get => _secondsLeft;
            set => SetField(ref _secondsLeft, value);
        }

        private double CalculateMoment(EventArgs e)
        {
            var serialPortEventArgs = (SerialPortEventArgs)e;
            return _model.DetermineMoment(serialPortEventArgs.Left, serialPortEventArgs.Right); 
        }

        private string CalculateVoltage(EventArgs e)
        {
            var serialPortEventArgs = (SerialPortEventArgs)e;
            return _model.DetermineVoltage(serialPortEventArgs.Left, serialPortEventArgs.Right);
        }

        private void StopCalibrationButton_Click(object sender, RoutedEventArgs e)
        {
            _model.Listener.NewMessage -= OnMessage; 
            _mainWindow.StartFrontPage(); 
        }

        private void OnMessageDuringCalibration(object sender, EventArgs e)
        {
            var moment = CalculateMoment(e); 
            _sumOfMoments += moment;
            _numberOfMoments++; 
        }

        double _sumOfMoments;
        int _numberOfMoments;
        Timer _timer; 

        private IntensityTypeEnum _calibratedValue; 

        private void StartCalibration()
        {
            LockButtons(true); 

            _sumOfMoments = 0;
            _numberOfMoments = 0;
            SecondsLeft = 10;

            _model.Listener.NewMessage += OnMessageDuringCalibration;

            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Interval = 1000;
            _timer.Elapsed += CalibrationTimerElapsed;
            _timer.Start(); 
        }

        private void CalibrationTimerElapsed(object sender, ElapsedEventArgs e)
        {
            SecondsLeft--;
            if (SecondsLeft == 0)
            {
                _timer.Stop();

                // Prevent event leaks.
                _timer.Elapsed -= CalibrationTimerElapsed;
                _model.Listener.NewMessage -= OnMessageDuringCalibration;

                // Calculate outcome of calibration.
                int avgMoment = (int)(_sumOfMoments / (double)_numberOfMoments);
                if (_calibratedValue == IntensityTypeEnum.Laag)
                {
                    _model.CurrentAthlete.MomentMin = avgMoment;
                }
                if (_calibratedValue == IntensityTypeEnum.Middel)
                {
                    _model.CurrentAthlete.MomentMid = avgMoment;
                }
                else if (_calibratedValue == IntensityTypeEnum.Hoog)
                {
                    _model.CurrentAthlete.MomentMax = avgMoment;
                }

                LockButtons(false);
            }
        }

        private void CalibrateMinButton_Click(object sender, RoutedEventArgs e)
        {
            _calibratedValue = IntensityTypeEnum.Laag;
            StartCalibration(); 
        }
        
        private void CalibrateMidButton_Click(object sender, RoutedEventArgs e)
        {
            _calibratedValue = IntensityTypeEnum.Middel;
            StartCalibration();
        }
        
        private void CalibrateMaxButton_Click(object sender, RoutedEventArgs e)
        {
            _calibratedValue = IntensityTypeEnum.Hoog;
            StartCalibration();
        }

        private void LockButtons(bool shouldLock)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                CalibrateMinButton.IsEnabled = !shouldLock;
                CalibrateMidButton.IsEnabled = !shouldLock;
                CalibrateMaxButton.IsEnabled = !shouldLock;
                StopCalibrationButton.IsEnabled = !shouldLock;
            });
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
