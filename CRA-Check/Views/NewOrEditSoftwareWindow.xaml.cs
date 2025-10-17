using CRA_Check.Models;
using MahApps.Metro.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;


namespace CRA_Check.Views
{
    /// <summary>
    /// Interaction logic for NewOrEditSoftwareWindow.xaml
    /// Window to create or edit a Software
    /// </summary>
    public partial class NewOrEditSoftwareWindow : MetroWindow, INotifyPropertyChanged
    {
        /// <summary>
        /// List of existing Software
        /// </summary>
        private ObservableCollection<Software> _softwares;
        
        /// <summary>
        /// Software original name in case of edition
        /// </summary>
        private string _originalName;

        /// <summary>
        /// Software name
        /// </summary>
        private string _softwareName;
        public string SoftwareName
        {
            get { return _softwareName; }
            set
            {
                _softwareName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// True it is in creation mode, False it is edition mode
        /// </summary>
        public bool IsCreationMode { get; private set; }

        /// <summary>
        /// Window title. Depend if it is in creation or edition
        /// </summary>
        public string WindowTitle { get; private set; } = "Edit software";

        /// <summary>
        /// True if the all information are valid
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="softwares">List of existing Software</param>
        /// <param name="name">Name of Software. By default is null. If is null => creation mode else edition mode</param>
        public NewOrEditSoftwareWindow(ObservableCollection<Software> softwares, string name = null)
        {
            _softwares = softwares;
            _originalName = name;

            SoftwareName = name;
            IsCreationMode = string.IsNullOrEmpty(name);
            if (IsCreationMode)
            {
                WindowTitle = "New software";
            }

            InitializeComponent();

            DataContext = this;
        }

        /// <summary>
        /// Action to apply the modification
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Not used</param>
        private void Apply_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SoftwareName))
            {
                System.Windows.MessageBox.Show("The name cannot be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(_originalName != SoftwareName && _softwares.Any(s => s.Name == SoftwareName))
            {
                System.Windows.MessageBox.Show("A software with the same name already exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            IsValid = true;
            Close();
        }

        /// <summary>
        /// Action to cancel the edition
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Not used</param>
        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
