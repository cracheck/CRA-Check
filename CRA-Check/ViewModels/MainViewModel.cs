using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CRA_Check.Data;
using CRA_Check.Models;

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
                }

                _softwares = value;

                _softwares.CollectionChanged += SoftwaresOnCollectionChanged;

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
                Softwares = new ObservableCollection<Software>(dbContext.Softwares.ToList());
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
                    foreach (var software in e.NewItems)
                    {
                        dbContext.Softwares.Add(software as Software);
                    }
                }

                if (e.OldItems != null)
                {
                    foreach (var software in e.OldItems)
                    {
                        dbContext.Softwares.Remove(software as Software);
                    }
                }

                dbContext.SaveChanges();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
