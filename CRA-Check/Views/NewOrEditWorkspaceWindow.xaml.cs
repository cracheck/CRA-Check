using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using MahApps.Metro.Controls;


namespace CRA_Check.Views
{
    /// <summary>
    /// Interaction logic for NewOrEditWorkspaceWindow.xaml
    /// Create or edit a Workspace
    /// </summary>
    public partial class NewOrEditWorkspaceWindow : MetroWindow, INotifyPropertyChanged
    {
        /// <summary>
        /// Workspace
        /// </summary>
        private string _workspaceName;
        public string WorkspaceName
        {
            get { return _workspaceName; }
            set
            {
                _workspaceName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Filename of the Workspace
        /// </summary>
        private string _filename;
        public string Filename
        {
            get { return _filename; }
            private set
            {
                _filename = value;
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
        public string WindowTitle { get; private set; } = "Edit workspace";

        /// <summary>
        /// True if the all information are valid
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of Workspace. By default is null. If is null => creation mode else edition mode</param>
        public NewOrEditWorkspaceWindow(string name = null)
        {
            WorkspaceName = name;
            IsCreationMode = string.IsNullOrEmpty(name);
            if (IsCreationMode)
            {
                WindowTitle = "New workspace";
            }

            InitializeComponent();

            DataContext = this;
        }

        /// <summary>
        /// Action to select the workspace path
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Not used</param>
        private void Browse_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Title = "Save as",
                Filter = "CRA DB file (*.cradb)|*.cradb",
                FileName = WorkspaceName,
                DefaultExt = ".cradb"
            };

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Filename = dialog.FileName;
            }
        }

        /// <summary>
        /// Action to apply the modification
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Not used</param>
        private void Apply_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(WorkspaceName))
            {
                System.Windows.MessageBox.Show("The name cannot be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (IsCreationMode && string.IsNullOrEmpty(Filename))
            {
                System.Windows.MessageBox.Show("Please select a filename", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
