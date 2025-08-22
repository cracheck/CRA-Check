using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace CRA_Check.Models
{
    public class Software : INotifyPropertyChanged
    {
        public int Id { get; set; }

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

        [NotMapped]
        public SeverityLevel MaxSeverityLevel
        {
            get { return Releases.Where(r => r.IsActive).DefaultIfEmpty(new Release()).Max(r => r.MaxSeverityLevel); }
        }

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
