using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace CRA_Check.Models
{
    /// <summary>
    /// Data model for release
    /// </summary>
    public class Release : INotifyPropertyChanged
    {
        /// <summary>
        /// Database ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Software
        /// </summary>
        public Software Software { get; set; }

        /// <summary>
        /// Software ID
        /// </summary>
        public int SoftwareId { get; set; }

        /// <summary>
        /// Release version as string
        /// </summary>
        public string VersionStr { get; set; }

        /// <summary>
        /// Release version
        /// </summary>
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

        /// <summary>
        /// SBOM
        /// </summary>
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

        /// <summary>
        /// Last scan date and time
        /// </summary>
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

        /// <summary>
        /// Is the release is active
        /// </summary>
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

        /// <summary>
        /// List of components
        /// </summary>
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

        /// <summary>
        /// Max severity level between all components
        /// </summary>
        [NotMapped]
        public SeverityLevel MaxSeverityLevel
        {
            get { return Components.DefaultIfEmpty(new Component()).Max(v => v.MaxSeverityLevel); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
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
