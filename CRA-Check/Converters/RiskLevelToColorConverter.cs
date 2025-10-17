using CRA_Check.Models;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace CRA_Check.Converters
{
    /// <summary>
    /// Converter. Convert SecurityLevel enum to Color
    /// </summary>
    public class RiskLevelToColorConverter : IValueConverter
    {
        /// <summary>
        /// Convert SecurityLevel enum to Color
        /// </summary>
        /// <param name="value">SecurityLevel value</param>
        /// <param name="targetType">Not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">Not used</param>
        /// <returns>Color</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SeverityLevel severityLevel = (SeverityLevel)value;

            switch (severityLevel)
            {
                case SeverityLevel.critical:
                    return new SolidColorBrush(Color.FromRgb(234, 99, 93));
                case SeverityLevel.high:
                    return new SolidColorBrush(Color.FromRgb(240, 150, 90));
                case SeverityLevel.medium:
                    return new SolidColorBrush(Color.FromRgb(244, 218, 107));
                case SeverityLevel.negligible:
                case SeverityLevel.low:
                    return new SolidColorBrush(Color.FromRgb(148, 216, 107));
                case SeverityLevel.noRisk:
                    return new SolidColorBrush(Colors.Green);
                case SeverityLevel.unknown:
                    return new SolidColorBrush(Colors.Gray);
            }

            return Colors.White;
        }

        /// <summary>
        /// Convert back. Color to SecurityLevel
        /// Not implemented
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
