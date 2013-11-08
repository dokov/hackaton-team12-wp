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
using MenuItem = Telerik.Everlive.Sdk.Sample.WindowsPhone.UI.MenuItem;

namespace Telerik.Everlive.Sdk.Sample.WindowsPhone
{
	public partial class MainMenuPage : PhoneApplicationPage
	{
		public MainMenuPage()
		{
			InitializeComponent();

			this.InitializeUI();

			this.InitializeData();
		}

		private void InitializeUI()
		{
			List<MenuItem> menuItems = new List<MenuItem>();
			menuItems.Add(new MenuItem("cafeteria menu", "see what's for lunch in Starbugs", "/UI/CafeteriaPage.xaml", "/Images/MainMenu/cafeteria.png"));
			menuItems.Add(new MenuItem("eat outside", "browse places for lunch outside Telerik", "/UI/UnderConstructionPage.xaml", "/Images/MainMenu/lunch.png"));
			menuItems.Add(new MenuItem("sport activities", "plan your daily dose of excercise", "/UI/UnderConstructionPage.xaml", "/Images/MainMenu/sports.png"));
			menuItems.Add(new MenuItem("company discounts", "learn about company discounts", "/UI/DiscountCategoriesMenuPage.xaml", "/Images/MainMenu/discounts.png"));
			menuItems.Add(new MenuItem("messages & promotions", "find out what happens in the company", "/UI/UnderConstructionPage.xaml", "/Images/MainMenu/messages.png"));
			this.Menu.ItemsSource = menuItems;
		}

		private void InitializeData()
		{
			
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
			{
				var backCount = this.NavigationService.BackStack.Count();
				for (int i = 0; i < backCount; i++)
				{
					this.NavigationService.RemoveBackEntry();
				}
			}
		}

		private void StackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			var menuItem = (MenuItem)((FrameworkElement)e.OriginalSource).DataContext;
			NavigationService.Navigate(menuItem.PageUrl, menuItem.Value);
		}


		#region Private Fields and Constants

		private bool isInitialized = false;

		#endregion

		private void LogoutButton_Click(object sender, EventArgs e)
		{
			AppManager.Instance.Authentication().Logout(delegate(Core.Result.EmptyResult result)
			{
				if (result.Success)
				{
				}
				this.Dispatcher.BeginInvoke(delegate()
				{
					this.NavigationService.Navigate(new Uri("/UI/LoginPage.xaml", UriKind.Relative));
				});
			});
		}
	}
}