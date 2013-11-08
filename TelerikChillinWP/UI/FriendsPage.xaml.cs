using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Telerik.Everlive.Sdk.Core.Linq.Translators;
using Telerik.Everlive.Sdk.Core.Query.Definition.Filtering;
using Telerik.Everlive.Sdk.Core.Query.Definition.Filtering.Simple;
using Telerik.Everlive.Sdk.Core.Query.Definition.Sorting;
using Telerik.Everlive.Sdk.Core.Result;
using Telerik.Everlive.Sdk.Sample.WindowsPhone.Model;

namespace Telerik.Everlive.Sdk.Sample.WindowsPhone
{
	public partial class FriendsPage : PhoneApplicationPage
	{
		public FriendsPage()
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
			AppManager.Instance.ShowProgress("loading friends...");

			//ValueCondition c = new ValueCondition("_id", ValueConditionOperator.ValueNotEquals, AppManager.Instance.LoggedUser.Id);

			AppManager.Instance.Users().Get().Where(u => u.Id != AppManager.Instance.LoggedUser.Id).OrderBy(u => u.DisplayName).TryExecute(
				delegate(RequestResult<IEnumerable<CustomUser>> result) {
					this.Dispatcher.BeginInvoke(delegate()
					{
						AppManager.Instance.HideProgress();

						if (result.Success)
						{
							this.FriendsList.ItemsSource = result.Value;
							this.NoFriendsText.Visibility = result.Value.Count() == 0 ? Visibility.Visible : Visibility.Collapsed;
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
			var user = (CustomUser)((FrameworkElement)e.OriginalSource).DataContext;
			NavigationService.Navigate("/UI/ViewProfilePage.xaml", user);
		}

		private void RefreshButton_Click(object sender, EventArgs e)
		{
			this.InitializeData();
		}

		private void SearchButton_Click(object sender, EventArgs e)
		{

		}

	}
}