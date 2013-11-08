using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Telerik.Everlive.Sdk.Sample.WindowsPhone.Resources
{
	public class LocalizedStrings
	{
		public LocalizedStrings() { }

		private static Resources localizedResources = new Resources();

		public Resources Strings { 
			get { 
				return localizedResources;
			} 
		}
	}
}
