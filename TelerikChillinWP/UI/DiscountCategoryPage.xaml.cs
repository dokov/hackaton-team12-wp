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
using Telerik.Everlive.Sdk.Core.Linq.Translators;
using Telerik.Everlive.Sdk.Core.Query.Definition;
using Telerik.Everlive.Sdk.Core.Query.Definition.Sorting;
using Telerik.Everlive.Sdk.Core.Result;
using Telerik.Everlive.Sdk.Sample.WindowsPhone.Model;

namespace Telerik.Everlive.Sdk.Sample.WindowsPhone
{
	public partial class DiscountCategoryPage : PhoneApplicationPage
	{
		public DiscountCategoryPage()
		{
			InitializeComponent();
		}

		private void InitializeUI(string categoryName)
		{
			this.PageTitle.Text = categoryName;
		}

		private void InitializeData(string categoryName)
		{
			AppManager.Instance.ShowProgress("loading discounts...");

			AppManager.Instance.Discounts().Get().Where(a => a.Category == categoryName).OrderBy(a => a.Title).TryExecute(
				delegate(RequestResult<IEnumerable<Discount>> result)
				{
					this.Dispatcher.BeginInvoke(delegate()
					{
						AppManager.Instance.HideProgress();
						if (result.Success)
						{
							this.ActivitiesList.ItemsSource = result.Value;
						}
						else
						{
							AppManager.Instance.ShowErrorMessage(result.Error.Message);
						}
					});
				}
			);
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
			{
				string categoryName = (string)NavigationContext.GetData();
				this.InitializeUI(categoryName);
				this.InitializeData(categoryName);
			}
		}

		private void StackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			var discount = (Discount)((FrameworkElement)e.OriginalSource).DataContext;
			NavigationService.Navigate("/UI/ViewDiscountPage.xaml", discount);
		}

	}
}