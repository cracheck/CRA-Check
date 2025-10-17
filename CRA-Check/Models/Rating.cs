using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CRA_Check.Models
{
    /// <summary>
    /// Data model for rating
    /// </summary>
    public class Rating : INotifyPropertyChanged
    {
        /// <summary>
        /// Database ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Vulnerability
        /// </summary>
        public Vulnerability Vulnerability { get; set; }

        /// <summary>
        /// Vulnerability ID
        /// </summary>
        public int VulnerabilityId { get; set; }

        /// <summary>
        /// Rating vector
        /// </summary>
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

        /// <summary>
        /// Rating method
        /// </summary>
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

        /// <summary>
        /// Rating score
        /// </summary>
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

        /// <summary>
        /// Severity level
        /// </summary>
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

        /// <summary>
        /// Constructor
        /// </summary>
        public Rating()
        {
            Severity = SeverityLevel.noRisk;
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
