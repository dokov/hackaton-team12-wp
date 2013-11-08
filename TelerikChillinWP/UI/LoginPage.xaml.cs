using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Telerik.Everlive.Sdk.Core.Result;
using Telerik.Everlive.Sdk.Sample.WindowsPhone.Model;

namespace Telerik.Everlive.Sdk.Sample.WindowsPhone
{
	public partial class LoginPage : PhoneApplicationPage
	{
		// Constructor
		public LoginPage()
		{
			InitializeComponent();
		}

		private void LoginButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			var pi = new ProgressIndicator();
			pi.IsIndeterminate = true;
			pi.Text = "logging in...";
			pi.IsVisible = true;
			SystemTray.ProgressIndicator = pi;

			AppManager.Instance.Authentication().Login(this.UsernameTextBox.Text, this.PasswordTextBox.Password, this.OnLoginResult);
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			var backCount = this.NavigationService.BackStack.Count();
			for (int i = 0; i < backCount; i++)
			{
				this.NavigationService.RemoveBackEntry();
			}

			string username, password;
			NavigationContext.QueryString.TryGetValue("Username", out username);
			NavigationContext.QueryString.TryGetValue("Password", out password);

			if (!string.IsNullOrWhiteSpace(username)) this.UsernameTextBox.Text = username;
			if (!string.IsNullOrWhiteSpace(password)) this.PasswordTextBox.Password = password;
		}

		/// <summary>
		/// Callback for server Login() command.
		/// </summary>
		/// <param name="result"></param>
		private void OnLoginResult(LoginResult result)
		{
			if (result.Success == true)
			{
				AppManager.Instance.IsLoggedIn = true;
				AppManager.Instance.Users().GetMe().TryExecute(this.OnGetMeResult);
			}
			else
			{
				AppManager.Instance.ShowErrorMessage(result.Error.Message);
			}
		}

		/// <summary>
		/// Callback for server GetMe() command.
		/// </summary>
		/// <param name="result"></param>
		private void OnGetMeResult(RequestResult<CustomUser> result)
		{
			this.Dispatcher.BeginInvoke(delegate()
			{
				var pi = SystemTray.ProgressIndicator;
				pi.IsVisible = false;

				if (result.Success == true)
				{
					AppManager.Instance.IsLoggedIn = true;
					AppManager.Instance.LoggedUser = result.Value;
					this.NavigationService.Navigate(new Uri("/UI/MainMenuPage.xaml", UriKind.Relative));
				}
				else
				{
					AppManager.Instance.ShowErrorMessage(result.Error.Message);
				}
			});
		}

		private void RegisterButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			NavigationService.Navigate("/UI/CreateAccountPage.xaml", new CustomUser());
		}
	}
}