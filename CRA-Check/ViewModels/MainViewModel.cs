using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using CRA_Check.Data;
using CRA_Check.Models;
using CRA_Check.Tools.ReportGenerators;
using CRA_Check.Tools.SbomGenerators;
using CRA_Check.Tools.VulnerabilityScanners;
using Microsoft.EntityFrameworkCore;
using DbContext = CRA_Check.Data.DbContext;

namespace CRA_Check.ViewModels
{
    /// <summary>
    /// Main view model
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Syft filename
        /// </summary>
        private static readonly string SYFT_FILEMAME = "syft.exe";
        /// <summary>
        /// Grype filename
        /// </summary>
        private static readonly string GRYPE_FILEMAME = "grype.exe";

        /// <summary>
        /// Database manager
        /// </summary>
        private DatabaseManager _databaseManager;

        /// <summary>
        /// SBOM generator
        /// </summary>
        public ISbomGenerator SbomGenerator { get; private set; }

        /// <summary>
        /// Vulnerability scanner
        /// </summary>
        public IVulnerabilityScanner VulnerabilityScanner { get; private set; }

        /// <summary>
        /// Vulnerability report generator
        /// </summary>
        public IReportGenerator ReportGenerator { get; private set; }

        /// <summary>
        /// Workspace information
        /// </summary>
        private WorkspaceInformation _workspaceInformation;
        public WorkspaceInformation WorkspaceInformation
        {
            get { return _workspaceInformation; }
            set
            {
                _workspaceInformation = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasWorkspace));
            }
        }

        /// <summary>
        /// True => a workspace is opened, False no workspace opened
        /// </summary>
        public bool HasWorkspace
        {
            get { return WorkspaceInformation != null; }
        }

        /// <summary>
        /// List of Software
        /// </summary>
        private ObservableCollection<Software> _softwares;
        public ObservableCollection<Software> Softwares
        {
            get { return _softwares; }
            set
            {
                if (_softwares != null)
                {
                    _softwares.CollectionChanged -= SoftwaresOnCollectionChanged;
                    foreach (var software in _softwares)
                    {
                        software.PropertyChanged -= SoftwareOnPropertyChanged;
                        software.Releases.CollectionChanged -= ReleasesOnCollectionChanged;

                        foreach (var release in software.Releases)
                        {
                            release.PropertyChanged -= ReleaseOnPropertyChanged;
                        }
                    }
                }

                _softwares = value;

                _softwares.CollectionChanged += SoftwaresOnCollectionChanged;
                foreach (var software in _softwares)
                {
                    software.PropertyChanged += SoftwareOnPropertyChanged;
                    software.Releases.CollectionChanged += ReleasesOnCollectionChanged;

                    foreach (var release in software.Releases)
                    {
                        release.PropertyChanged += ReleaseOnPropertyChanged;
                    }
                }

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Constructor
        /// Initialize Syft, Grype and report generator
        /// </summary>
        public MainViewModel()
        {
            _databaseManager = new DatabaseManager();

            string exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            SbomGenerator = new SyftSbomGenerator(Path.Combine(exeDirectory, SYFT_FILEMAME));
            VulnerabilityScanner = new GrypeScanner(Path.Combine(exeDirectory, GRYPE_FILEMAME));

            ReportGenerator = new PdfReportGenerator();

#if DEBUG
            OpenWorkspace(@"d:\test.cradb");
#endif
        }

        /// <summary>
        /// Open a workspace
        /// </summary>
        /// <param name="filename">Workspace filename</param>
        public void OpenWorkspace(string filename)
        {
            _databaseManager.ChangeDatabase(filename);

            if (WorkspaceInformation != null)
            {
                CloseWorkspace();
            }

            using (DbContext dbContext = _databaseManager.GetContext())
            {
                Softwares = new ObservableCollection<Software>(dbContext.Softwares
                    .Include(s => s.Releases)
                    .ThenInclude(r => r.Components)
                    .ThenInclude(c => c.Vulnerabilities)
                    .ThenInclude(v => v.Ratings).ToList());

                WorkspaceInformation = dbContext.WorkspaceInformation.First();
                WorkspaceInformation.PropertyChanged += WorkspaceInformationOnPropertyChanged;
            }
        }

        /// <summary>
        /// Close the current workspace
        /// </summary>
        public void CloseWorkspace()
        {
            WorkspaceInformation.PropertyChanged -= WorkspaceInformationOnPropertyChanged;
        }

        /// <summary>
        /// Create a new workspace
        /// </summary>
        /// <param name="filename">Workspace filename</param>
        /// <param name="name">Workspace name</param>
        public void CreateWorkspace(string filename, string name)
        {
            _databaseManager.ChangeDatabase(filename);

            using (DbContext dbContext = _databaseManager.GetContext())
            {
                dbContext.WorkspaceInformation.Add(new WorkspaceInformation() { Name = name });
                dbContext.SaveChanges();
            }

            OpenWorkspace(filename);
        }

        /// <summary>
        /// Action if workspace is modified
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Not used</param>
        private void WorkspaceInformationOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            using (DbContext dbContext = _databaseManager.GetContext())
            {
                dbContext.WorkspaceInformation.First().Name = WorkspaceInformation.Name;
                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Action if softwares are modified
        /// </summary>
        /// <param name="sender">Modified Software</param>
        /// <param name="e">Not used</param>
        private void SoftwareOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            Software software = sender as Software;

            if (software != null)
            {
                using (DbContext dbContext = _databaseManager.GetContext())
                {
                    dbContext.Softwares.First(s => s.Id == software.Id).Name = software.Name;
                    dbContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Action if softwares list is modified
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Information of added/removed software</param>
        private void SoftwaresOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            using (DbContext dbContext = _databaseManager.GetContext())
            {
                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems)
                    {
                        Software software = item as Software;
                        if (software != null)
                        {
                            dbContext.Softwares.Add(software);

                            software.Releases.CollectionChanged += ReleasesOnCollectionChanged;
                            software.PropertyChanged += SoftwareOnPropertyChanged;

                            foreach (var release in software.Releases)
                            {
                                release.PropertyChanged += ReleaseOnPropertyChanged;
                            }
                        }
                    }
                }

                if (e.OldItems != null)
                {
                    foreach (var item in e.OldItems)
                    {
                        Software software = item as Software;
                        if (software != null)
                        {
                            dbContext.Softwares.Remove(software);

                            software.Releases.CollectionChanged -= ReleasesOnCollectionChanged;
                            software.PropertyChanged -= SoftwareOnPropertyChanged;

                            foreach (var release in software.Releases)
                            {
                                release.PropertyChanged -= ReleaseOnPropertyChanged;
                            }
                        }
                    }
                }

                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Action if release list is modified
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Information of added/removed release</param>
        private void ReleasesOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            using (DbContext dbContext = _databaseManager.GetContext())
            {
                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems)
                    {
                        Release release = item as Release;
                        if (release != null)
                        {
                            var software = dbContext.Softwares.Include(s => s.Releases)
                                .FirstOrDefault(s => s.Id == release.Software.Id);

                            if (software != null)
                            {
                                software.Releases.Add(release);
                            }
                        }

                        release.PropertyChanged += ReleaseOnPropertyChanged;
                    }

                    if (e.OldItems != null)
                    {
                        foreach (var item in e.OldItems)
                        {
                            Release release = item as Release;
                            if (release != null)
                            {
                                var software = dbContext.Softwares.Include(s => s.Releases)
                                    .FirstOrDefault(s => s.Id == release.Software.Id);

                                if (software != null)
                                {
                                    software.Releases.Remove(release);
                                }
                            }

                            release.PropertyChanged -= ReleaseOnPropertyChanged;
                        }
                    }

                    dbContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Action if releases are modified
        /// </summary>
        /// <param name="sender">Modified Release</param>
        /// <param name="e">Not used</param>
        private void ReleaseOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            Release newRelease = sender as Release;

            if (newRelease != null)
            {
                using (DbContext dbContext = _databaseManager.GetContext())
                {
                    Release release = dbContext.Releases
                        .Include(r => r.Components)
                        .ThenInclude(c => c.Vulnerabilities)
                        .ThenInclude(v => v.Ratings)
                        .FirstOrDefault(r => r.Id == newRelease.Id);

                    if (release != null)
                    {
                        if (e.PropertyName == nameof(Release.Components))
                        {
                            dbContext.Components.RemoveRange(release.Components);

                            // Remove unused Vulnerability
                            //var unusedVulnerabilities = dbContext.Vulnerabilities.Where()

                            foreach (var component in newRelease.Components)
                            {
                                component.Release = release;
                            }

                            dbContext.Components.AddRange(newRelease.Components);
                        }
                        else
                        {
                            dbContext.Entry(release).CurrentValues.SetValues(newRelease);
                        }
                    }

                    dbContext.SaveChanges();
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
