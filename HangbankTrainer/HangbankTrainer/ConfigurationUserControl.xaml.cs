using System.Windows.Controls;

namespace HangbankTrainer
{
    /// <summary>
    /// Interaction logic for ConfigurationUserControl.xaml
    /// </summary>
    public partial class ConfigurationUserControl : UserControl
    {

        private HangbankMainWindow _mainWindow; 
        private HangbankModel _model;

        public ConfigurationUserControl(HangbankMainWindow mainWindow, HangbankModel model)
        {
            _mainWindow = mainWindow; 
            _model = model; 
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComPortComboBox.SelectedValue != null)
            {
                string comPort = ComPortComboBox.SelectedValue.ToString();
                _model.SetSerialPort(comPort);
            }
        }
    }
}
