using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CRA_Check.Models
{
    public class Cvss : INotifyPropertyChanged
    {
        public int Id { get; set; }

        public Vulnerability Vulnerability { get; set; }

        public int VulnerabilityId { get; set; }


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

        private string _source;
        public string Source
        {
            get { return _source; }
            set
            {
                _source = value;
                OnPropertyChanged();
            }
        }

        private double _baseScore;
        public double BaseScore
        {
            get { return _baseScore; }
            set
            {
                _baseScore = value;
                OnPropertyChanged();
            }
        }

        private double _impactScore;
        public double ImpactScore
        {
            get { return _impactScore; }
            set
            {
                _impactScore = value;
                OnPropertyChanged();
            }
        }

        private double _exploitabilityScore;
        public double ExploitabilityScore
        {
            get { return _exploitabilityScore; }
            set
            {
                _exploitabilityScore = value;
                OnPropertyChanged();
            }
        }

        public Cvss()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
