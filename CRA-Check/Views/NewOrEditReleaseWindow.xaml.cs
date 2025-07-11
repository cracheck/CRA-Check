using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;


namespace CRA_Check.Views
{
    /// <summary>
    /// Interaction logic for NewOrEditReleaseWindow.xaml
    /// </summary>
    public partial class NewOrEditReleaseWindow : Window, INotifyPropertyChanged
    {
        private string _versionStr;
        public string VersionStr
        {
            get { return _versionStr; }
            set
            {
                _versionStr = value;
                OnPropertyChanged();
            }
        }

        // TODO change with boolean
        private string _SbomStatus;
        public string SbomStatus
        {
            get { return _SbomStatus; }
            set
            {
                _SbomStatus = value;
                OnPropertyChanged();
            }
        }

        public Version Version { get; private set; }

        public string Sbom { get; private set; }

        public bool IsValid { get; private set; }

        public NewOrEditReleaseWindow()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void CreateSoftware_OnClick(object sender, RoutedEventArgs e)
        {
            // TODO test unique
            Version version;
            if (Version.TryParse(VersionStr, out version))
            {
                Version = version;
            }
            else
            {
                System.Windows.MessageBox.Show("The version number is not valid.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (SbomStatus != "OK")
            {
                System.Windows.MessageBox.Show("The sbom is not valid", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void ScanVersion_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Title = "Open file", Filter = "Executable files (*.exe;*.dll)|*.exe;*.dll|All files (*.*)|*.*" };

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileVersionInfo info = FileVersionInfo.GetVersionInfo(openFileDialog.FileName);
                if (info != null)
                {
                    if (!string.IsNullOrEmpty(info.ProductVersion))
                    {
                        VersionStr = info.ProductVersion;
                    }
                    else if (!string.IsNullOrEmpty(info.FileVersion))
                    {
                        VersionStr = info.FileVersion;
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("No version information found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void ImportSbom_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Title = "Open file", Filter = "Sbom file (*.json)|*.json" };

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string sbom = File.ReadAllText(openFileDialog.FileName);

                // TODO check validity. With Syft ?

                Sbom = sbom;
                SbomStatus = "OK";
            }
        }

        private void ScanSbom_OnClick(object sender, RoutedEventArgs e)
        {
            // TODO with Syft
        }
    }
}
