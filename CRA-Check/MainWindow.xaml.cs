using System.Windows;
using CRA_Check.Data;
using CRA_Check.Models;
using CRA_Check.ViewModels;
using CRA_Check.Views;
using Microsoft.Win32;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace CRA_Check
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel MainViewModel { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            //TryDb();

            MainViewModel = new MainViewModel();

            DataContext = MainViewModel;
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

            software.Release.Add(release1);
            software.Release.Add(release2);
            software2.Release.Add(release3);

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
            NewOrEditWorkspaceWindow window = new NewOrEditWorkspaceWindow();
            window.ShowDialog();

            if (window.IsValid)
            {
                MainViewModel.CreateWorkspace(window.Filename, window.Name);
            }
        }

        private void AddSoftware_OnClick(object sender, RoutedEventArgs e)
        {
            NewOrEditSoftwareWindow window = new NewOrEditSoftwareWindow();
            window.ShowDialog();

            if (window.IsValid)
            {
                MainViewModel.Softwares.Add(new Software() { Name = window.SoftwareName });
            }
        }
    }
}