using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Telerik.Everlive.Sdk.Core.Query.Definition.Updating;
using Telerik.Everlive.Sdk.Core.Result;
using Telerik.Everlive.Sdk.Sample.WindowsPhone.Model;

namespace Telerik.Everlive.Sdk.Sample.WindowsPhone.UI
{
	public partial class ViewDiscountPage : PhoneApplicationPage
	{

		public ViewDiscountPage()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
			{
				var discount = (Discount)NavigationContext.GetData();
				if (discount != null)
				{
					discount.LoadFiles();
					this.DataContext = discount;
				}
			}
		}
	}
}