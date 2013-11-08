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
using Telerik.Everlive.Sdk.Sample.WindowsPhone.Model;

namespace Telerik.Everlive.Sdk.Sample.WindowsPhone.UI
{
	public partial class ViewProfilePage : PhoneApplicationPage
	{


		public ViewProfilePage()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
			{
				var user = (CustomUser)NavigationContext.GetData();
				if (user != null)
				{
					this.DataContext = user;
					this.currentUser = user;
					this.RefreshApplicationBar();
				}
			}
		}

		private void RefreshApplicationBar()
		{
			var appBar =  this.ApplicationBar;


			var befriendButton = new ApplicationBarIconButton() {
				IconUri = new Uri("Images/appbar_befriend.png", UriKind.Relative),
				Text = "befriend"
			};
			befriendButton.Click += new EventHandler(LikeButton_Click);

			var unfriendButton = new ApplicationBarIconButton()
			{
				IconUri = new Uri("Images/appbar_unfriend.png", UriKind.Relative),
				Text = "unfriend"
			};
			unfriendButton.Click += new EventHandler(UnlikeButton_Click);

			var editButton = new ApplicationBarIconButton() {
				IconUri = new Uri("Images/appbar_edit.png", UriKind.Relative),
				Text = "edit"
			};
			editButton.Click += new EventHandler(EditButton_Click);

			bool isOwnProfile = this.currentUser.Id == AppManager.Instance.LoggedUser.Id;

			appBar.Buttons.Clear();

			if (isOwnProfile)
			{
				appBar.Buttons.Add(editButton);
			}
			else
			{
				//appBar.Buttons.Add(befriendButton);
				//appBar.Buttons.Add(unfriendButton);
			}
		}

		private void LikeButton_Click(object sender, EventArgs e)
		{
			
		}

		private void UnlikeButton_Click(object sender, EventArgs e)
		{
			
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			NavigationService.Navigate("/UI/CreateAccountPage.xaml", this.currentUser);
		}

		#region Private Fields and Constants

		private CustomUser currentUser;

		#endregion
	}
}