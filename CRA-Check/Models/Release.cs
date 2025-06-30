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

        private ObservableCollection<Vulnerability> _vulnerabilities;
        public ObservableCollection<Vulnerability> Vulnerabilities
        {
            get { return _vulnerabilities; }
            set
            {
                _vulnerabilities = value;
                OnPropertyChanged();
            }
        }

        public Release()
        {
            Vulnerabilities = new ObservableCollection<Vulnerability>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string _PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_PropertyName));
        }
    }
}
