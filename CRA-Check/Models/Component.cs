using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace CRA_Check.Models
{
    public class Component : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public Release Release { get; set; }
        public int ReleaseId { get; set; }

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

        [NotMapped]
        public double MaxVulnerabilityRating
        {
            get { return Vulnerabilities.DefaultIfEmpty(new Vulnerability()).Max(v => v.MaxRating); }
        }

        [NotMapped]
        public SeverityLevel MaxSeverityLevel
        {
            get { return Vulnerabilities.DefaultIfEmpty(new Vulnerability()).Max(v => v.MaxSeverityLevel); }
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
