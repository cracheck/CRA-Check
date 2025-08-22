using CRA_Check.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using UserControl = System.Windows.Controls.UserControl;

namespace CRA_Check.Views
{
    /// <summary>
    /// Interaction logic for WorkspaceStatusControl.xaml
    /// </summary>
    public partial class WorkspaceStatusControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty SoftwaresProperty = DependencyProperty.Register("Softwares", typeof(ObservableCollection<Software>), typeof(WorkspaceStatusControl), new FrameworkPropertyMetadata(new ObservableCollection<Software>(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SoftwaresPropertyChangedCallback));
        private static void SoftwaresPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WorkspaceStatusControl control = d as WorkspaceStatusControl;
            if (control != null)
            {
                control.UpdateValues();
            }
        }

        public ObservableCollection<Software> Softwares
        {
            get { return (ObservableCollection<Software>)GetValue(SoftwaresProperty); }
            set { SetValue(SoftwaresProperty, value); }
        }

        private int _SoftwareCriticalNumber;
        public int SoftwareCriticalNumber
        {
            get { return _SoftwareCriticalNumber; }
            set
            {
                _SoftwareCriticalNumber = value;
                OnPropertyChanged();
            }
        }

        private int _SoftwareHighNumber;
        public int SoftwareHighNumber
        {
            get { return _SoftwareHighNumber; }
            set
            {
                _SoftwareHighNumber = value;
                OnPropertyChanged();
            }
        }

        private int _SoftwareMediumNumber;
        public int SoftwareMediumNumber
        {
            get { return _SoftwareMediumNumber; }
            set
            {
                _SoftwareMediumNumber = value;
                OnPropertyChanged();
            }
        }

        private int _SoftwareLowNumber;
        public int SoftwareLowNumber
        {
            get { return _SoftwareLowNumber; }
            set
            {
                _SoftwareLowNumber = value;
                OnPropertyChanged();
            }
        }

        private int _SoftwareNoRiskNumber;
        public int SoftwareNoRiskNumber
        {
            get { return _SoftwareNoRiskNumber; }
            set
            {
                _SoftwareNoRiskNumber = value;
                OnPropertyChanged();
            }
        }

        private int _SoftwareUnknowNumber;
        public int SoftwareUnknowNumber
        {
            get { return _SoftwareUnknowNumber; }
            set
            {
                _SoftwareUnknowNumber = value;
                OnPropertyChanged();
            }
        }

        private int _ReleaseCriticalNumber;
        public int ReleaseCriticalNumber
        {
            get { return _ReleaseCriticalNumber; }
            set
            {
                _ReleaseCriticalNumber = value;
                OnPropertyChanged();
            }
        }

        private int _ReleaseHighNumber;
        public int ReleaseHighNumber
        {
            get { return _ReleaseHighNumber; }
            set
            {
                _ReleaseHighNumber = value;
                OnPropertyChanged();
            }
        }

        private int _ReleaseMediumNumber;
        public int ReleaseMediumNumber
        {
            get { return _ReleaseMediumNumber; }
            set
            {
                _ReleaseMediumNumber = value;
                OnPropertyChanged();
            }
        }

        private int _ReleaseLowNumber;
        public int ReleaseLowNumber
        {
            get { return _ReleaseLowNumber; }
            set
            {
                _ReleaseLowNumber = value;
                OnPropertyChanged();
            }
        }

        private int _ReleaseNoRiskNumber;
        public int ReleaseNoRiskNumber
        {
            get { return _ReleaseNoRiskNumber; }
            set
            {
                _ReleaseNoRiskNumber = value;
                OnPropertyChanged();
            }
        }

        private int _ReleaseUnknowNumber;
        public int ReleaseUnknowNumber
        {
            get { return _ReleaseUnknowNumber; }
            set
            {
                _ReleaseUnknowNumber = value;
                OnPropertyChanged();
            }
        }

        private int _ComponentCriticalNumber;
        public int ComponentCriticalNumber
        {
            get { return _ComponentCriticalNumber; }
            set
            {
                _ComponentCriticalNumber = value;
                OnPropertyChanged();
            }
        }

        private int _ComponentHighNumber;
        public int ComponentHighNumber
        {
            get { return _ComponentHighNumber; }
            set
            {
                _ComponentHighNumber = value;
                OnPropertyChanged();
            }
        }

        private int _ComponentMediumNumber;
        public int ComponentMediumNumber
        {
            get { return _ComponentMediumNumber; }
            set
            {
                _ComponentMediumNumber = value;
                OnPropertyChanged();
            }
        }

        private int _ComponentLowNumber;
        public int ComponentLowNumber
        {
            get { return _ComponentLowNumber; }
            set
            {
                _ComponentLowNumber = value;
                OnPropertyChanged();
            }
        }

        private int _ComponentNoRiskNumber;
        public int ComponentNoRiskNumber
        {
            get { return _ComponentNoRiskNumber; }
            set
            {
                _ComponentNoRiskNumber = value;
                OnPropertyChanged();
            }
        }

        private int _ComponentUnknowNumber;
        public int ComponentUnknowNumber
        {
            get { return _ComponentUnknowNumber; }
            set
            {
                _ComponentUnknowNumber = value;
                OnPropertyChanged();
            }
        }

        private DateTime _LastScan;
        public DateTime LastScan
        {
            get { return _LastScan; }
            set
            {
                _LastScan = value;
                OnPropertyChanged();
            }
        }

        public WorkspaceStatusControl()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void UpdateValues()
        {
            SoftwareCriticalNumber = 0;
            SoftwareHighNumber = 0;
            SoftwareMediumNumber = 0;
            SoftwareLowNumber = 0;
            SoftwareNoRiskNumber = 0;
            SoftwareUnknowNumber = 0;

            ReleaseCriticalNumber = 0;
            ReleaseHighNumber = 0;
            ReleaseMediumNumber = 0;
            ReleaseLowNumber = 0;
            ReleaseNoRiskNumber = 0;
            ReleaseUnknowNumber = 0;

            ComponentCriticalNumber = 0;
            ComponentHighNumber = 0;
            ComponentMediumNumber = 0;
            ComponentLowNumber = 0;
            ComponentNoRiskNumber = 0;
            ComponentUnknowNumber = 0;

            LastScan = DateTime.MinValue;

            foreach (var software in Softwares)
            {
                switch (software.MaxSeverityLevel)
                {
                    case SeverityLevel.critical:
                        SoftwareCriticalNumber++;
                        break;
                    case SeverityLevel.high:
                        SoftwareHighNumber++;
                        break;
                    case SeverityLevel.medium:
                        SoftwareMediumNumber++;
                        break;
                    case SeverityLevel.negligible:
                    case SeverityLevel.low:
                        SoftwareLowNumber++;
                        break;
                    case SeverityLevel.unknown:
                        SoftwareUnknowNumber++;
                        break;
                    case SeverityLevel.noRisk:
                        SoftwareNoRiskNumber++;
                        break;
                }

                foreach (var release in software.Releases)
                {
                    if (!release.IsActive)
                    {
                        continue;
                    }

                    switch (release.MaxSeverityLevel)
                    {
                        case SeverityLevel.critical:
                            ReleaseCriticalNumber++;
                            break;
                        case SeverityLevel.high:
                            ReleaseHighNumber++;
                            break;
                        case SeverityLevel.medium:
                            ReleaseMediumNumber++;
                            break;
                        case SeverityLevel.negligible:
                        case SeverityLevel.low:
                            ReleaseLowNumber++;
                            break;
                        case SeverityLevel.unknown:
                            ReleaseUnknowNumber++;
                            break;
                        case SeverityLevel.noRisk:
                            ReleaseNoRiskNumber++;
                            break;
                    }

                    if (LastScan == DateTime.MinValue || LastScan > release.LastScan)
                    {
                        LastScan = release.LastScan;
                    }

                    foreach (var component in release.Components)    
                    {
                        switch (component.MaxSeverityLevel)
                        {
                            case SeverityLevel.critical:
                                ComponentCriticalNumber++;
                                break;
                            case SeverityLevel.high:
                                ComponentHighNumber++;
                                break;
                            case SeverityLevel.medium:
                                ComponentMediumNumber++;
                                break;
                            case SeverityLevel.negligible:
                            case SeverityLevel.low:
                                ComponentLowNumber++;
                                break;
                            case SeverityLevel.unknown:
                                ComponentUnknowNumber++;
                                break;
                            case SeverityLevel.noRisk:
                                ComponentNoRiskNumber++;
                                break;
                        }
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
