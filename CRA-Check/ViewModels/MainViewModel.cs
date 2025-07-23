using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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
    public class MainViewModel : INotifyPropertyChanged
    {
        private DatabaseManager _databaseManager;
        public ISbomGenerator SbomGenerator { get; private set; }
        public IVulnerabilityScanner VulnerabilityScanner { get; private set; }
        public IReportGenerator ReportGenerator { get; private set; }

        private WorkspaceInformation _workspaceInformation;
        public WorkspaceInformation WorkspaceInformation
        {
            get { return _workspaceInformation; }
            set
            {
                _workspaceInformation = value;
                OnPropertyChanged();
            }
        }

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

        public MainViewModel()
        {
            _databaseManager = new DatabaseManager();
            SbomGenerator = new SyftSbomGenerator(@"D:\Syft.exe"); // TODO change
            VulnerabilityScanner = new GrypeScanner(@"D:\Grype.exe"); // TODO change
            ReportGenerator = new PdfReportGenerator();
            OpenWorkspace(@"d:\test.cradb"); // TODO Remove
        }

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
                    .ThenInclude(r => r.Vulnerabilities)
                    .ThenInclude(v => v.Ratings).ToList());

                WorkspaceInformation = dbContext.WorkspaceInformation.First();
                WorkspaceInformation.PropertyChanged += WorkspaceInformationOnPropertyChanged;
            }
        }

        public void CloseWorkspace()
        {
            WorkspaceInformation.PropertyChanged -= WorkspaceInformationOnPropertyChanged;
        }

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

        private void WorkspaceInformationOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            using (DbContext dbContext = _databaseManager.GetContext())
            {
                dbContext.WorkspaceInformation.First().Name = WorkspaceInformation.Name;
                dbContext.SaveChanges();
            }
        }

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

        private void ReleaseOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            Release newRelease = sender as Release;

            if (newRelease != null)
            {
                using (DbContext dbContext = _databaseManager.GetContext())
                {
                    Release release = dbContext.Releases
                        .Include(r => r.Vulnerabilities)
                        .ThenInclude(v => v.Ratings)
                        .FirstOrDefault(r => r.Id == newRelease.Id);

                    if (release != null)
                    {
                        if (e.PropertyName == nameof(Release.Vulnerabilities))
                        {
                            dbContext.Vulnerabilities.RemoveRange(release.Vulnerabilities);

                            foreach (var vulnerability in newRelease.Vulnerabilities)
                            {
                                vulnerability.Release = release;
                                foreach (var rating in vulnerability.Ratings)
                                {
                                    rating.Vulnerability = vulnerability;
                                }
                            }

                            dbContext.Vulnerabilities.AddRange(newRelease.Vulnerabilities);
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
