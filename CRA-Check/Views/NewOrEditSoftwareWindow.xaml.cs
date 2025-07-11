using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;


namespace CRA_Check.Views
{
    /// <summary>
    /// Interaction logic for NewOrEditSoftwareWindow.xaml
    /// </summary>
    public partial class NewOrEditSoftwareWindow : Window, INotifyPropertyChanged
    {
        private string _softwareName;
        public string SoftwareName
        {
            get { return _softwareName; }
            set
            {
                _softwareName = value;
                OnPropertyChanged();
            }
        }

        public bool IsValid { get; private set; }

        public NewOrEditSoftwareWindow()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void CreateSoftware_OnClick(object sender, RoutedEventArgs e)
        {
            // TODO test unique
            if (SoftwareName == "")
            {
                System.Windows.MessageBox.Show("The WorkspaceName cannot be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            IsValid = true;
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
