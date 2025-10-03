using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CRA_Check.Converters
{
    internal sealed class BooleanToVisibilityCustomConverter : IValueConverter
	{
	    public BooleanToVisibilityCustomConverter()
	    {
	        VisibilityOnTrue = Visibility.Visible;
	        VisibilityOnFalse = Visibility.Collapsed;
	    }

		public Visibility VisibilityOnTrue { get; set; }

		public Visibility VisibilityOnFalse { get; set; }

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

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
