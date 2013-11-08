using System;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Telerik.Everlive.Sdk.Sample.WindowsPhone.Converters
{
	public class ImageSourceToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			bool hideWhenNull = "true".Equals(parameter);
			ImageSource imgSource = (ImageSource)value;
			Visibility result = hideWhenNull ? Visibility.Visible : Visibility.Collapsed;
			if (imgSource == null)
			{
				result = hideWhenNull ? Visibility.Collapsed : Visibility.Visible;
			}

			if (parameter == "")
			{

			}

			return result;
		}

		//One way binding doesnt need this
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return this.Convert(value, targetType, parameter, new CultureInfo(language));
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
