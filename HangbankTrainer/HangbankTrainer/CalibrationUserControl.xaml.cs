using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
            var eventArgs = (SerialPortEventArgs)e;

            var linksMoment = (int)(1000 * (eventArgs.Left - _model.LinksOnbelast) / (_model.LinksBelast - _model.LinksOnbelast));

            Value = linksMoment; 
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

        private void StopCalibrationButton_Click(object sender, RoutedEventArgs e)
        {
            _model.Listener.NewMessage -= OnMessage;
            _mainWindow.StartFrontPage(); 
        }
        
        private void CalibrateMinButton_Click(object sender, RoutedEventArgs e)
        {
            _model.CurrentAthlete.MomentMin = _value;  
        }
        
        private void CalibrateGameButton_Click(object sender, RoutedEventArgs e)
        {
            _model.CurrentAthlete.MomentGame = _value;
        }
        
        private void CalibrateMaxButton_Click(object sender, RoutedEventArgs e)
        {
            _model.CurrentAthlete.MomentMax = _value;
        }
    }
}
