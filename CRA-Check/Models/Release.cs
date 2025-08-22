using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace CRA_Check.Models
{
    public class Release : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public Software Software { get; set; }
        public int SoftwareId { get; set; }

        public string VersionStr { get; set; }

        [NotMapped]
        public Version Version
        {
            get { return Version.TryParse(VersionStr, out var v) ? v : new Version(0, 0); }
            set
            {
                VersionStr = value.ToString();
                OnPropertyChanged();
            }
        }

        // May be a JSON object in the futur
        private string _sbom;
        public string Sbom
        {
            get { return _sbom; }
            set
            {
                _sbom = value;
                OnPropertyChanged();
            }
        }

        private DateTime _lastScan;
        public DateTime LastScan
        {
            get { return _lastScan; }
            set
            {
                _lastScan = value;
                OnPropertyChanged();
            }
        }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Component> _components;
        public ObservableCollection<Component> Components
        {
            get { return _components; }
            set
            {
                _components = value;
                OnPropertyChanged();
            }
        }

        [NotMapped]
        public SeverityLevel MaxSeverityLevel
        {
            get { return Components.DefaultIfEmpty(new Component()).Max(v => v.MaxSeverityLevel); }
        }

        public Release()
        {
            Components = new ObservableCollection<Component>();
            IsActive = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
