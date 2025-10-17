using MahApps.Metro.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CRA_Check.Views
{
    /// <summary>
    /// Interaction logic for LoadingWindow.xaml
    /// Loading window
    /// </summary>
    public partial class LoadingWindow : MetroWindow, INotifyPropertyChanged
    {
        /// <summary>
        /// Waiting message
        /// </summary>
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

        /// <summary>
        /// Constructor
        /// </summary>
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
