using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using MahApps.Metro.Controls;


namespace CRA_Check.Views
{
    /// <summary>
    /// Interaction logic for NewOrEditSoftwareWindow.xaml
    /// </summary>
    public partial class NewOrEditSoftwareWindow : MetroWindow, INotifyPropertyChanged
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

        public bool IsCreationMode { get; private set; }

        public string WindowTitle { get; private set; } = "Edit software";

        public bool IsValid { get; private set; }

        public NewOrEditSoftwareWindow(string name = null)
        {
            SoftwareName = name;
            IsCreationMode = string.IsNullOrEmpty(name);
            if (IsCreationMode)
            {
                WindowTitle = "New software";
            }

            InitializeComponent();

            DataContext = this;
        }

        private void Apply_OnClick(object sender, RoutedEventArgs e)
        {
            // TODO test unique
            if (string.IsNullOrEmpty(SoftwareName))
            {
                System.Windows.MessageBox.Show("The name cannot be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            IsValid = true;
            Close();
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
