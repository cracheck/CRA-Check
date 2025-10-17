using CRA_Check.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using UserControl = System.Windows.Controls.UserControl;

namespace CRA_Check.Views
{
    /// <summary>
    /// Interaction logic for ReleaseStatusControl.xaml
    /// Overview of a Release. Resume the vulnerabilities of the Release
    /// </summary>
    public partial class ReleaseStatusControl : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// Release
        /// </summary>
        public static readonly DependencyProperty ReleaseProperty = DependencyProperty.Register("Release", typeof(Release), typeof(ReleaseStatusControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ReleasePropertyChangedCallback));
        private static void ReleasePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ReleaseStatusControl control = d as ReleaseStatusControl;
            if (control != null)
            {
                control.UpdateValues();
            }
        }

        /// <summary>
        /// Release
        /// </summary>
        public Release Release
        {
            get { return (Release)GetValue(ReleaseProperty); }
            set { SetValue(ReleaseProperty, value); }
        }

        /// <summary>
        /// Number of component with critical vulnerability
        /// </summary>
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
        
        /// <summary>
        /// Number of component with high vulnerability
        /// </summary>
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

        /// <summary>
        /// Number of component with medium vulnerability
        /// </summary>
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

        /// <summary>
        /// Number of component with low vulnerability
        /// </summary>
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

        /// <summary>
        /// Number of component with no vulnerability
        /// </summary>
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

        /// <summary>
        /// Number of component with unknow vulnerability
        /// </summary>
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

        /// <summary>
        /// Last scan of the release
        /// </summary>
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

        /// <summary>
        /// Constructor
        /// </summary>
        public ReleaseStatusControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Update value after a scan
        /// </summary>
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
