using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CRA_Check.Models
{
    public class Project : INotifyPropertyChanged
    {
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

        private ObservableCollection<Software> m_Softwares;
        public ObservableCollection<Software> Softwares
        {
            get { return m_Softwares; }
            set
            {
                m_Softwares = value;
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
