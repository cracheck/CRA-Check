using CRA_Check.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using UserControl = System.Windows.Controls.UserControl;

namespace CRA_Check.Views
{
    /// <summary>
    /// Interaction logic for WorkspaceStatusControl.xaml
    /// </summary>
    public partial class ReleaseStatusControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ReleaseProperty = DependencyProperty.Register("Release", typeof(Release), typeof(ReleaseStatusControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ReleasePropertyChangedCallback));
        private static void ReleasePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ReleaseStatusControl control = d as ReleaseStatusControl;
            if (control != null)
            {
                control.UpdateValues();
            }
        }

        public Release Release
        {
            get { return (Release)GetValue(ReleaseProperty); }
            set { SetValue(ReleaseProperty, value); }
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

        public ReleaseStatusControl()
        {
            InitializeComponent();

           // DataContext = this;
        }

        private void UpdateValues()
        {
            ComponentCriticalNumber = 0;
            ComponentHighNumber = 0;
            ComponentMediumNumber = 0;
            ComponentLowNumber = 0;
            ComponentNoRiskNumber = 0;
            ComponentUnknowNumber = 0;

            LastScan = DateTime.MinValue;

            if (Release == null)
            {
                return;
            }

            LastScan = Release.LastScan;

            foreach (var component in Release.Components)
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
