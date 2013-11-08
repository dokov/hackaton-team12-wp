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
	public partial class PlacesPage : PhoneApplicationPage
	{
		public PlacesPage()
		{
			InitializeComponent();

			this.InitializeUI();

			this.InitializeData();
		}

		private void InitializeUI()
		{
			
		}

		private void InitializeData()
		{
			AppManager.Instance.ShowProgress("loading places...");

			AppManager.Instance.Activities().Get().OrderByDescending(a => a.CreatedAt).Take(5).TryExecute(
				delegate(RequestResult<IEnumerable<Activity>> result)
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

			if (AppManager.Instance.MustRefreshActivities)
			{
				AppManager.Instance.MustRefreshActivities = false;
				this.InitializeData();
			}
		}

		private void StackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			var activity = (Activity)((FrameworkElement)e.OriginalSource).DataContext;
			NavigationService.Navigate("/UI/ViewActivityPage.xaml", activity);
		}

		private void AddActivityButton_Click(object sender, EventArgs e)
		{
			NavigationService.Navigate("/UI/CreateOrEditActivityPage.xaml", new Activity());
		}

		private void RefreshButton_Click(object sender, EventArgs e)
		{
			this.InitializeData();
		}

	}
}