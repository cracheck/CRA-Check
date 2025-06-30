using System.Windows;
using CRA_Check.Data;
using CRA_Check.Models;
using CRA_Check.ViewModels;
using Microsoft.Win32;

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

            bool? status = openFileDialog.ShowDialog(this);

            if (status != null && (bool) status)
            {
                MainViewModel.OpenWorkspace(openFileDialog.FileName);
            }
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}