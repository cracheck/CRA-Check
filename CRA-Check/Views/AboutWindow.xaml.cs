using MahApps.Metro.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CRA_Check.Views
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// About window
    /// </summary>
    public partial class AboutWindow : MetroWindow, INotifyPropertyChanged
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AboutWindow()
        {
            InitializeComponent();

            DataContext = this;
        }

        /// <summary>
        /// Version of the application
        /// </summary>
        private Version _version;
        public Version Version
        {
            get { return _version;}
            set
            {
                _version = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Version of Syft
        /// </summary>
        private Version _syftVersion;
        public Version SyftVersion
        {
            get { return _syftVersion;}
            set
            {
                _syftVersion = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Version of Grype.exe
        /// </summary>
        private Version _grypeVersion;
        public Version GrypeVersion
        {
            get { return _grypeVersion; }
            set
            {
                _grypeVersion = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
