using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CRA_Check.Models
{
    public class Release : INotifyPropertyChanged
    {
        private Version m_Version;
        public Version Version
        {
            get { return m_Version; }
            set
            {
                m_Version = value;
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string _PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_PropertyName));
        }
    }
}
