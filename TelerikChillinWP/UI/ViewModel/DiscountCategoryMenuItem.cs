using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Telerik.Everlive.Sdk.Sample.WindowsPhone.UI
{
	public class DiscountCategoryMenuItem
	{

		#region Properties

		public string Label { get; set; }

		//public string Category { get; set; }

		public string IconPath { get; set; }

		#endregion

		#region Constructors and Initialization

		public DiscountCategoryMenuItem()
		{

		}

		public DiscountCategoryMenuItem(string label, string iconPath)
		{
			this.Label = label;
			//this.Category = category;
			this.IconPath = iconPath;
		}

		#endregion

		public ImageSource Icon
		{
			get
			{
				return new BitmapImage(new Uri("/Images/Categories/" + this.IconPath, UriKind.Relative));
			}
		}
	}
}
