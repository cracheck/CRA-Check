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
        private string m_Sbom;
        public string Sbom
        {
            get { return m_Sbom; }
            set
            {
                m_Sbom = value;
                OnPropertyChanged();
            }
        }

        private DateTime m_LastScan;
        public DateTime LastScan
        {
            get { return m_LastScan; }
            set
            {
                m_LastScan = value;
                OnPropertyChanged();
            }
        }

        private bool m_IsActive;
        public bool IsActive
        {
            get { return m_IsActive; }
            set
            {
                m_IsActive = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Vulnerability> m_Vulnerabilities;
        public ObservableCollection<Vulnerability> Vulnerabilities
        {
            get { return m_Vulnerabilities; }
            set
            {
                m_Vulnerabilities = value;
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
