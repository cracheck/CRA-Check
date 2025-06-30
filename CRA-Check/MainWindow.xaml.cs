using System.Windows;
using CRA_Check.Data;
using CRA_Check.Models;

namespace CRA_Check
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //TryDb();
        }

        private void TryDb()
        {
            using var db = new AppDbContext(@"d:\app.db");
            db.Database.EnsureCreated();

            var projectInformation = new ProjectInformation() { Name = "Proj1" };
            var software = new Software() { Name = "Soft1" };
            var software2 = new Software() { Name = "Soft2" };
            var release1 = new Release() { Version = new Version(3, 2), Sbom = "sbom1" };
            var release2 = new Release() { Version = new Version(3, 3), Sbom = "sbom2" };
            var release3 = new Release() { Version = new Version(4, 3), Sbom = "sbom3" };

            software.Release.Add(release1);
            software.Release.Add(release2);
            software2.Release.Add(release3);

            db.ProjectInformation.Add(projectInformation);
            db.Softwares.Add(software);
            db.Softwares.Add(software2);
            db.SaveChanges();
        }
    }
}