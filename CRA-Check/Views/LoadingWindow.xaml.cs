using MahApps.Metro.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CRA_Check.Views
{
    /// <summary>
    /// Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : MetroWindow, INotifyPropertyChanged
    {
        private string _waitingMessage;

        public string WaitingMessage
        {
            get { return _waitingMessage; }
            set
            {
                _waitingMessage = value; 
                OnPropertyChanged();
            }
        }

        public LoadingWindow()
        {
            InitializeComponent();

            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
