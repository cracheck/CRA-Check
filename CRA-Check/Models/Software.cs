using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CRA_Check.Models
{
    public class Software : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set
            {
                m_Name = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Release> m_Releases;
        public ObservableCollection<Release> Release
        {
            get { return m_Releases; }
            set
            {
                m_Releases = value;
                OnPropertyChanged();
            }
        }

        public Software()
        {
            Release = new ObservableCollection<Release>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string _PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_PropertyName));
        }
    }
}
