using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;


namespace CRA_Check.Views
{
    /// <summary>
    /// Interaction logic for NewOrEditWorkspaceWindow.xaml
    /// </summary>
    public partial class NewOrEditWorkspaceWindow : Window, INotifyPropertyChanged
    {
        private string _workspaceName;
        public string WorkspaceName
        {
            get { return _workspaceName; }
            set
            {
                _workspaceName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Filename));
            }
        }

        private string _path;
        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Filename));
            }
        }

        public string Filename
        {
            get { return System.IO.Path.Combine(Path, Name + ".cradb"); }
        }

        public bool IsValid { get; private set; }

        public NewOrEditWorkspaceWindow()
        {
            InitializeComponent();

            Name = "";
            Path = "";

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
                    Path = dialog.SelectedPath;
                }
            }
        }

        private void CreateWorkspace_OnClick(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(Path) || Path == "")
            {
                System.Windows.MessageBox.Show("The folder does not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Name == "")
            {
                System.Windows.MessageBox.Show("The WorkspaceName cannot be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
