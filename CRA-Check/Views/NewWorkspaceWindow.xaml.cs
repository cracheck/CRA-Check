using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;


namespace CRA_Check.Views
{
    /// <summary>
    /// Interaction logic for NewWorkspaceWindow.xaml
    /// </summary>
    public partial class NewWorkspaceWindow : Window, INotifyPropertyChanged
    {
        private string _workspaceName;
        public string WorkspaceName
        {
            get { return _workspaceName; }
            set
            {
                _workspaceName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(WorkspaceFilename));
            }
        }

        private string _workspacePath;
        public string WorkspacePath
        {
            get { return _workspacePath; }
            set
            {
                _workspacePath = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(WorkspaceFilename));
            }
        }

        public string WorkspaceFilename
        {
            get { return Path.Combine(WorkspacePath, WorkspaceName + ".cradb"); }
        }

        public bool IsValid { get; private set; }

        public NewWorkspaceWindow()
        {
            InitializeComponent();

            WorkspaceName = "";
            WorkspacePath = "";

            DataContext = this;
        }

        private void Browse_OnClick(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select a folder";
                dialog.UseDescriptionForTitle = true;

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    WorkspacePath = dialog.SelectedPath;
                }
            }
        }

        private void CreateWorkspace_OnClick(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(WorkspacePath) || WorkspacePath == "")
            {
                System.Windows.MessageBox.Show("The folder does not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (WorkspaceName == "")
            {
                System.Windows.MessageBox.Show("The name cannot be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            IsValid = true;
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
