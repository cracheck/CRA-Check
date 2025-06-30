using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CRA_Check.Data;
using CRA_Check.Models;

namespace CRA_Check.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private DatabaseManager _databaseManager;

        private ObservableCollection<Software> _softwares;
        public ObservableCollection<Software> Softwares
        {
            get { return _softwares; }
            set
            {
                _softwares = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            _databaseManager = new DatabaseManager();
        }

        public void OpenWorkspace(string filename)
        {
            _databaseManager.ChangeDatabase(filename);

            using (DbContext dbContext = _databaseManager.GetContext())
            {
                Softwares = new ObservableCollection<Software>(dbContext.Softwares.ToList());
            }
        }

        public void CreateWorkspace(string filename, string name)
        {
            OpenWorkspace(filename);
            using (DbContext dbContext = _databaseManager.GetContext())
            {
                dbContext.WorkspaceInformation.Add(new WorkspaceInformation(){Name = name});
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
