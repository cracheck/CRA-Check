using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CRA_Check.Converters
{
    /// <summary>
	/// Bool to Visibility converter. With possibility to configure the Visibility on True and False
	/// </summary>
    internal sealed class BooleanToVisibilityCustomConverter : IValueConverter
	{
		/// <summary>
		/// Constructor
		/// By default:
		/// True => Visible
		/// False => Collapsed
		/// </summary>
	    public BooleanToVisibilityCustomConverter()
	    {
	        VisibilityOnTrue = Visibility.Visible;
	        VisibilityOnFalse = Visibility.Collapsed;
	    }

		/// <summary>
		/// Visibility if True
		/// </summary>
		public Visibility VisibilityOnTrue { get; set; }

		/// <summary>
		/// Visibility if False
		/// </summary>
		public Visibility VisibilityOnFalse { get; set; }

		/// <summary>
		/// Convert bool value to Visibility
		/// </summary>
		/// <param name="value">Boolean value</param>
		/// <param name="targetType">Not used</param>
		/// <param name="parameter">Not used</param>
		/// <param name="culture">Not used</param>
		/// <returns>Visibility</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool)
			{
				if ((bool) value)
				{
					return VisibilityOnTrue;
				}
				else
				{
					return VisibilityOnFalse;
				}
			}
			return null;
		}

		/// <summary>
		/// Convert back. Visibility to bool
		/// Not implemented
		/// </summary>
		/// <exception cref="NotImplementedException"></exception>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
