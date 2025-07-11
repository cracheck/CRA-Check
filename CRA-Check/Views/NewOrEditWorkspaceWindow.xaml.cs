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
            }
        }

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

        public bool IsCreationMode { get; private set; }

        public string WindowTitle { get; private set; } = "Edit workspace";

        public bool IsValid { get; private set; }

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
