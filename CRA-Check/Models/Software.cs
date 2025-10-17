using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace CRA_Check.Models
{
    /// <summary>
    /// Data model for software
    /// </summary>
    public class Software : INotifyPropertyChanged
    {
        /// <summary>
        /// Database ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name
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
        /// List of Release
        /// </summary>
        private ObservableCollection<Release> _releases;
        public ObservableCollection<Release> Releases
        {
            get { return _releases; }
            set
            {
                _releases = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Max SeverityLevel between all Release
        /// </summary>
        [NotMapped]
        public SeverityLevel MaxSeverityLevel
        {
            get { return Releases.Where(r => r.IsActive).DefaultIfEmpty(new Release()).Max(r => r.MaxSeverityLevel); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Software()
        {
            Releases = new ObservableCollection<Release>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
