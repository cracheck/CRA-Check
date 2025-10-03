using CRA_Check.Models;
using CRA_Check.ViewModels;
using MahApps.Metro.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;


namespace CRA_Check.Views
{
    /// <summary>
    /// Interaction logic for NewOrEditSoftwareWindow.xaml
    /// </summary>
    public partial class NewOrEditSoftwareWindow : MetroWindow, INotifyPropertyChanged
    {
        private ObservableCollection<Software> _softwares;
        private string _originalName;

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

        public bool IsCreationMode { get; private set; }

        public string WindowTitle { get; private set; } = "Edit software";

        public bool IsValid { get; private set; }

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
