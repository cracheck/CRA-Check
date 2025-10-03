using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using CRA_Check.Models;
using CRA_Check.Tools.SbomGenerators;
using CRA_Check.ViewModels;
using MahApps.Metro.Controls;


namespace CRA_Check.Views
{
    /// <summary>
    /// Interaction logic for NewOrEditReleaseWindow.xaml
    /// </summary>
    public partial class NewOrEditReleaseWindow : MetroWindow, INotifyPropertyChanged
    {
        private Software _software;
        private ISbomGenerator _sbomGenerator;

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

        private bool _SbomStatus;
        public bool SbomStatus
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

        public NewOrEditReleaseWindow(Software software, ISbomGenerator sbomGenerator)
        {
            _software = software;
            _sbomGenerator = sbomGenerator;

            InitializeComponent();

            DataContext = this;
        }

        private void Create_OnClick(object sender, RoutedEventArgs e)
        {
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

            if (_software.Releases.Any(r => r.Version == Version))
            {
                System.Windows.MessageBox.Show("This version number already exist for this software.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!SbomStatus)
            {
                System.Windows.MessageBox.Show("The sbom is not valid", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            IsValid = true;
            Close();
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
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

                if (CheckSbom(sbom))
                {
                    Sbom = sbom;
                    SbomStatus = true;
                }
            }
        }

        private bool CheckSbom(string sbom)
        {
            // TODO check validity. With Syft ?

            return true;
        }

        private async void ScanSbom_OnClick(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadingWindow window = new LoadingWindow();

                window.Show();

                window.WaitingMessage = "Creating SBOM";
                
                string sbom = await _sbomGenerator.GenerateSbom(dialog.SelectedPath);

                window.Close();

                if (CheckSbom(sbom))
                {
                    Sbom = sbom;
                    SbomStatus = true;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
