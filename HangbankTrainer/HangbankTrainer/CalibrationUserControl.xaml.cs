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
        }

        private int _value; 
        public int Value
        {
            get => _value;
            set => SetField(ref _value, value); 
        }

        private void OnMessage(object sender, EventArgs e)
        {
            Value = CalculateMoment(e); 
        }

        private int _secondsLeft; 
        public int SecondsLeft
        {
            get => _secondsLeft;
            set => SetField(ref _secondsLeft, value);
        }

        private int CalculateMoment(EventArgs e)
        {
            var serialPortEventArgs = (SerialPortEventArgs)e;
            return (int)(1000 * (serialPortEventArgs.Left - _model.LinksOnbelast) / (_model.LinksBelast - _model.LinksOnbelast));
        }

        private void StopCalibrationButton_Click(object sender, RoutedEventArgs e)
        {
            _model.Listener.NewMessage -= OnMessage; 
            _mainWindow.StartFrontPage(); 
        }

        private void OnMessageDuringCalibration(object sender, EventArgs e)
        {
            // TODO: Differentiate left/right. 
            var linksMoment = CalculateMoment(e); 

            _sumOfMoments += linksMoment;
            _numberOfMoments++; 
        }

        long _sumOfMoments;
        int _numberOfMoments;
        Timer _timer; 

        enum CalibrationValue
        {
            Min, Game, Max
        };

        private CalibrationValue _calibratedValue; 

        private void StartCalibration()
        {
            LockButtons(true); 

            _sumOfMoments = 0;
            _numberOfMoments = 0;
            SecondsLeft = 10;

            _model.Listener.NewMessage += OnMessageDuringCalibration;

            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Interval = 500;
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
                if (_calibratedValue == CalibrationValue.Min)
                {
                    _model.CurrentAthlete.MomentMin = avgMoment;
                }
                if (_calibratedValue == CalibrationValue.Game)
                {
                    _model.CurrentAthlete.MomentGame = avgMoment;
                }
                else if (_calibratedValue == CalibrationValue.Max)
                {
                    _model.CurrentAthlete.MomentMax = avgMoment;
                }

                LockButtons(false);
            }
        }

        private void CalibrateMinButton_Click(object sender, RoutedEventArgs e)
        {
            _calibratedValue = CalibrationValue.Min;
            StartCalibration(); 
        }
        
        private void CalibrateGameButton_Click(object sender, RoutedEventArgs e)
        {
            _calibratedValue = CalibrationValue.Game;
            StartCalibration();
        }
        
        private void CalibrateMaxButton_Click(object sender, RoutedEventArgs e)
        {
            _calibratedValue = CalibrationValue.Max;
            StartCalibration();
        }

        private void LockButtons(bool shouldLock)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                CalibrateMinButton.IsEnabled = !shouldLock;
                CalibrateGameButton.IsEnabled = !shouldLock;
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
