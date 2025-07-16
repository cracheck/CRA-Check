using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CRA_Check.Models
{
    public class Rating : INotifyPropertyChanged
    {
        public int Id { get; set; }

        public Vulnerability Vulnerability { get; set; }

        public int VulnerabilityId { get; set; }

        private string _vector;
        public string Vector
        {
            get { return _vector; }
            set
            {
                _vector = value;
                OnPropertyChanged();
            }
        }

        private string _method;
        public string Method
        {
            get { return _method; }
            set
            {
                _method = value;
                OnPropertyChanged();
            }
        }

        private double _Score;
        public double Score
        {
            get { return _Score; }
            set
            {
                _Score = value;
                OnPropertyChanged();
            }
        }

        private SeverityLevel _severity;
        public SeverityLevel Severity
        {
            get { return _severity; }
            set
            {
                _severity = value;
                OnPropertyChanged();
            }
        }

        public Rating()
        {
            Score = -1;
            Vector = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
