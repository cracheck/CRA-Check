using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace CRA_Check.Models
{
    /// <summary>
    /// Data model for component
    /// </summary>
    public class Component : INotifyPropertyChanged
    {
        /// <summary>
        /// Database ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Release
        /// </summary>
        public Release Release { get; set; }

        /// <summary>
        /// Release ID
        /// </summary>
        public int ReleaseId { get; set; }

        /// <summary>
        /// Component name
        /// </summary>
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Version as string
        /// </summary>
        private string _version;
        public string Version
        {
            get { return _version; }
            set
            {
                _version = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Max vulnerability rating between all vulnerabilities
        /// </summary>
        [NotMapped]
        public double MaxVulnerabilityRating
        {
            get { return Vulnerabilities.DefaultIfEmpty(new Vulnerability()).Max(v => v.MaxRating); }
        }

        /// <summary>
        /// Max severity level between all vulnerabilities
        /// </summary>
        [NotMapped]
        public SeverityLevel MaxSeverityLevel
        {
            get { return Vulnerabilities.DefaultIfEmpty(new Vulnerability()).Max(v => v.MaxSeverityLevel); }
        }

        /// <summary>
        /// List of Vulnerability
        /// </summary>
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

        /// <summary>
        /// Constructor
        /// </summary>
        public Component()
        {
            Vulnerabilities = new ObservableCollection<Vulnerability>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
