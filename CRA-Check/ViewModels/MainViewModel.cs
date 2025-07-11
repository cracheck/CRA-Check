using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CRA_Check.Data;
using CRA_Check.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using DbContext = CRA_Check.Data.DbContext;

namespace CRA_Check.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private DatabaseManager _databaseManager;

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
                        software.Releases.CollectionChanged -= ReleasesOnCollectionChanged;
                    }
                }

                _softwares = value;

                _softwares.CollectionChanged += SoftwaresOnCollectionChanged;
                foreach (var software in _softwares)
                {
                    software.Releases.CollectionChanged += ReleasesOnCollectionChanged;
                }

                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            _databaseManager = new DatabaseManager();
            OpenWorkspace(@"d:\test.cradb"); // TODO Remove
        }

        public void OpenWorkspace(string filename)
        {
            _databaseManager.ChangeDatabase(filename);

            using (DbContext dbContext = _databaseManager.GetContext())
            {
                Softwares = new ObservableCollection<Software>(dbContext.Softwares.Include(s => s.Releases).ToList());

                WorkspaceInformation = dbContext.WorkspaceInformation.First();
            }
        }

        public void CreateWorkspace(string filename, string name)
        {
            OpenWorkspace(filename);
            using (DbContext dbContext = _databaseManager.GetContext())
            {
                dbContext.WorkspaceInformation.Add(new WorkspaceInformation() { Name = name });
                dbContext.SaveChanges();
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
                            dbContext.Softwares.Add(software);
                            software.Releases.CollectionChanged -= ReleasesOnCollectionChanged;
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
