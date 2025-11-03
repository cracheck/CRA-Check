using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using CRA_Check.Models;
using CRA_Check.Tools.Parser;
using CRA_Check.Tools.ReportGenerators;
using CRA_Check.Tools.SbomGenerators;
using CRA_Check.Tools.VulnerabilityScanners;
using MahApps.Metro.Controls;


namespace CRA_Check.Views
{
    /// <summary>
    /// Interaction logic for GenerateReportWindow.xaml
    /// Generate a vulnerability report from an SBOM
    /// </summary>
    public partial class GenerateReportWindow : MetroWindow, INotifyPropertyChanged
    {
        /// <summary>
        /// SBOM generator
        /// </summary>
        private ISbomGenerator _sbomGenerator;

        /// <summary>
        /// Vulnerability scanner
        /// </summary>
        private IVulnerabilityScanner _vulnerabilityScanner;

        /// <summary>
        /// Report generator
        /// </summary>
        private IReportGenerator _reportGenerator;

        /// <summary>
        /// Software name
        /// </summary>
        private string _SoftwareName;
        public string SoftwareName
        {
            get { return _SoftwareName; }
            set
            {
                _SoftwareName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Version as string of the Release
        /// </summary>
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

        /// <summary>
        /// SBOM status. True => SBOM is valid, False => SBOM not valid
        /// </summary>
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

        /// <summary>
        /// Version of the Release
        /// </summary>
        public Version Version { get; private set; }

        /// <summary>
        /// SBOM
        /// </summary>
        public string Sbom { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sbomGenerator">SBOM generator</param>
        /// <param name="vulnerabilityScanner"> Vulnerability scanner</param>
        /// <param name="reportGenerator"> Vulnerability scanner</param>
        public GenerateReportWindow(ISbomGenerator sbomGenerator, IVulnerabilityScanner vulnerabilityScanner, IReportGenerator reportGenerator)
        {
            _sbomGenerator = sbomGenerator;
            _vulnerabilityScanner = vulnerabilityScanner;
            _reportGenerator = reportGenerator;

            InitializeComponent();

            DataContext = this;
        }

        /// <summary>
        /// Action to generate a vulnerabilities report
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Not used</param>
        private async void GenerateReport_OnClick(object sender, RoutedEventArgs e)
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

            if (string.IsNullOrEmpty(SoftwareName))
            {
                System.Windows.MessageBox.Show("The software name cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!SbomStatus)
            {
                System.Windows.MessageBox.Show("The SBOM is not valid.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Release release = new Release() {Sbom = Sbom, Software = new Software(){Name = SoftwareName}, Version = Version};

            LoadingWindow window = new LoadingWindow();

            window.Show();

            window.WaitingMessage = "Update database";

            await _vulnerabilityScanner.UpdateDatabase();

            window.WaitingMessage = "Scan for vulnerabilities";

            string scanResult = await _vulnerabilityScanner.ScanVulnerability(release);

            List<Models.Component> components = CycloneDXParser.ParseComponents(scanResult);

            release.Components = new ObservableCollection<Models.Component>(components);

            window.Close();

            SaveFileDialog saveDialog = new SaveFileDialog()
            {
                Title = "Save vulenrability report",
                Filter = "Vulnerability report (*.pdf)|*.pdf",
                DefaultExt = ".pdf"
            };

            if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _reportGenerator.GenerateReport(release, saveDialog.FileName);
            }

            Close();
        }

        /// <summary>
        /// Action to cancel the creation
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Not used</param>
        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Action to scan an *.exe file to get the version number
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Not used</param>
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

        /// <summary>
        /// Action to import an SBOM file
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Not used</param>
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

        /// <summary>
        /// Check if the SBOM is valid
        /// </summary>
        /// <param name="sbom">SBOM</param>
        /// <returns>True => SBOM valid, False => SBOM not valid</returns>
        private bool CheckSbom(string sbom)
        {
            // TODO check validity. With Syft ?

            return true;
        }

        /// <summary>
        /// Action to generate an SBOM from a folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
