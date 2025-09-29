using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows;
using CRA_Check.Data;
using CRA_Check.Models;
using CRA_Check.Tools.Parser;
using CRA_Check.ViewModels;
using CRA_Check.Views;
using MahApps.Metro.Controls;
using MessageBox = System.Windows.Forms.MessageBox;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace CRA_Check
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainViewModel MainViewModel { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            if (!File.Exists(@"d:\test.cradb"))
            {
                TryDb();
            }

            MainViewModel = new MainViewModel();

            DataContext = MainViewModel;
        }

        async private void ScanReleases(List<Release> releases)
        {
            LoadingWindow window = new LoadingWindow();

            window.Show();

            window.WaitingMessage = "Update database";

            await MainViewModel.VulnerabilityScanner.UpdateDatabase();

            foreach (var release in releases)
            {
                window.WaitingMessage = $"{release.Software.Name} {release.VersionStr}: Scan for vulnerabilities";

                string scanResult = await MainViewModel.VulnerabilityScanner.ScanVulnerability(release);

                List<Component> components = CycloneDXParser.ParseComponents(scanResult);

                release.LastScan = DateTime.Now;
                release.Components = new ObservableCollection<Component>(components);
            }

            window.Close();
        }

        private void TryDb()
        {
            using var db = new DbContext(@"d:\test.cradb");
            db.Database.EnsureCreated();

            var projectInformation = new WorkspaceInformation() { Name = "Workspace1" };
            var software = new Software() { Name = "Soft1" };
            var software2 = new Software() { Name = "Soft2" };
            var release1 = new Release() { Version = new Version(3, 2), Sbom = "sbom1" };
            var release2 = new Release() { Version = new Version(3, 3), Sbom = "sbom2" };
            var release3 = new Release() { Version = new Version(4, 3), Sbom = "sbom3" };

            software.Releases.Add(release1);
            software.Releases.Add(release2);
            software2.Releases.Add(release3);

            db.WorkspaceInformation.Add(projectInformation);
            db.Softwares.Add(software);
            db.Softwares.Add(software2);
            db.SaveChanges();
        }

        private void OpenWorkspace_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Choose a workspace",
                Filter = "CRA workspace (*.cradb)|*.cradb",
                Multiselect = false
            };

            var result = openFileDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                MainViewModel.OpenWorkspace(openFileDialog.FileName);
            }
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NewWorkspace_OnClick(object sender, RoutedEventArgs e)
        {
            NewOrEditWorkspaceWindow window = new NewOrEditWorkspaceWindow() { Owner = this };
            window.ShowDialog();

            if (window.IsValid)
            {
                MainViewModel.CreateWorkspace(window.Filename, window.WorkspaceName);
            }
        }

        async private void ScanWorkspace_OnClick(object sender, RoutedEventArgs e)
        {
            List<Release> releases = new List<Release>();
            foreach (var software in MainViewModel.Softwares)
            {
                foreach (var release in software.Releases)
                {
                    if (release.IsActive)
                    {
                        releases.Add(release);
                    }
                }
            }

            ScanReleases(releases);
        }

        private void AddSoftware_OnClick(object sender, RoutedEventArgs e)
        {
            NewOrEditSoftwareWindow window = new NewOrEditSoftwareWindow() { Owner = this };
            window.ShowDialog();

            if (window.IsValid)
            {
                MainViewModel.Softwares.Add(new Software() { Name = window.SoftwareName });
            }
        }

        async private void ScanSoftware_OnClick(object sender, RoutedEventArgs e)
        {
            FrameworkElement control = sender as FrameworkElement;
            if (control != null)
            {
                Software software = control.Tag as Software;
                if (software != null)
                {
                    ScanReleases(software.Releases.Where(r => r.IsActive).ToList());
                }
            }
        }

        private void AddRelease_OnClick(object sender, RoutedEventArgs e)
        {
            NewOrEditReleaseWindow window = new NewOrEditReleaseWindow(MainViewModel.SbomGenerator) { Owner = this };
            window.ShowDialog();

            if (window.IsValid)
            {
                FrameworkElement control = sender as FrameworkElement;
                if (control != null)
                {
                    Software software = control.Tag as Software;
                    if (software != null)
                    {
                        software.Releases.Add(new Release() { Version = window.Version, Sbom = window.Sbom, Software = software });
                    }
                }
            }
        }

        private void EditWorkspace_OnClick(object sender, RoutedEventArgs e)
        {
            NewOrEditWorkspaceWindow window = new NewOrEditWorkspaceWindow(MainViewModel.WorkspaceInformation.Name) { Owner = this };
            window.ShowDialog();

            if (window.IsValid)
            {
                MainViewModel.WorkspaceInformation.Name = window.WorkspaceName;
            }
        }

        private void EditSoftware_OnClick(object sender, RoutedEventArgs e)
        {
            FrameworkElement control = sender as FrameworkElement;
            if (control != null)
            {
                Software software = control.Tag as Software;
                if (software != null)
                {
                    NewOrEditSoftwareWindow window = new NewOrEditSoftwareWindow(software.Name);
                    window.ShowDialog();

                    if (window.IsValid)
                    {
                        software.Name = window.SoftwareName;
                    }
                }
            }
        }

        private void DeleteSoftware_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to remove this software?", "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) !=
                System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            FrameworkElement control = sender as FrameworkElement;
            if (control != null)
            {
                Software software = control.Tag as Software;
                if (software != null)
                {
                    MainViewModel.Softwares.Remove(software);
                }
            }
        }

        private async void ScanVulnerability_OnClick(object sender, RoutedEventArgs e)
        {
            FrameworkElement control = sender as FrameworkElement;
            if (control != null)
            {
                Release release = control.Tag as Release;
                if (release != null)
                {
                    ScanReleases(new List<Release>() { release });
                }
            }
        }

        private async void Detail_OnClick(object sender, RoutedEventArgs e)
        {
            FrameworkElement control = sender as FrameworkElement;
            if (control != null)
            {
                Release release = control.Tag as Release;
                if (release != null)
                {
                    ReleaseVulnerabilityDetailWindow window = new ReleaseVulnerabilityDetailWindow(MainViewModel, release);
                    window.ShowDialog();
                }
            }
        }

        private async void About_OnClick(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow() { Owner = this };

            window.Version = Assembly.GetExecutingAssembly().GetName().Version;
            window.SyftVersion = await MainViewModel.SbomGenerator.GetVersion();
            window.GrypeVersion = await MainViewModel.VulnerabilityScanner.GetVersion();

            window.ShowDialog();
        }
    }
}