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
using Telerik.Everlive.Sdk.Sample.WindowsPhone.UI;
using MenuItem = Telerik.Everlive.Sdk.Sample.WindowsPhone.UI.MenuItem;

namespace Telerik.Everlive.Sdk.Sample.WindowsPhone
{
	public partial class DiscountCategoriesMenuPage : PhoneApplicationPage
	{
		public DiscountCategoriesMenuPage()
		{
			InitializeComponent();

			this.InitializeUI();

			this.InitializeData();
		}

		private void InitializeUI()
		{

			List<DiscountCategoryMenuItem> menuItems = new List<DiscountCategoryMenuItem>();
			menuItems.Add(new DiscountCategoryMenuItem("Hair Studios & Cosmetics", "hair.png"));
			menuItems.Add(new DiscountCategoryMenuItem("Rest & Relaxation", "rest.png"));
			menuItems.Add(new DiscountCategoryMenuItem("Optical", "optical.png"));
			menuItems.Add(new DiscountCategoryMenuItem("Health & Fitness", "health.png"));
			menuItems.Add(new DiscountCategoryMenuItem("Jewelry and Watches", "jewelry.png"));
			menuItems.Add(new DiscountCategoryMenuItem("Home Services", "home.png"));
			menuItems.Add(new DiscountCategoryMenuItem("Finances", "finances.png"));
			menuItems.Add(new DiscountCategoryMenuItem("Children & Education", "children.png"));
			menuItems.Add(new DiscountCategoryMenuItem("Eating out", "eating.png"));
			menuItems.Add(new DiscountCategoryMenuItem("Entertainment", "entertainment.png"));
			menuItems.Add(new DiscountCategoryMenuItem("Automotive Services", "automotive.png"));
			menuItems.Add(new DiscountCategoryMenuItem("Travel and Leisure", "travel.png"));
			menuItems.Add(new DiscountCategoryMenuItem("Computers and Gadgets", "computers.png"));
			menuItems.Add(new DiscountCategoryMenuItem("Internet, Telephone and TV Providers", "internet.png"));
			this.Menu.ItemsSource = menuItems;
		}

		private void InitializeData()
		{
			
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			var backCount = this.NavigationService.BackStack.Count();
			for (int i = 0; i < backCount; i++)
			{
				this.NavigationService.RemoveBackEntry();
			}
		}

		private void StackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			var menuItem = (DiscountCategoryMenuItem)((FrameworkElement)e.OriginalSource).DataContext;
			NavigationService.Navigate("/UI/DiscountCategoryPage.xaml", menuItem.Label);
		}


		#region Private Fields and Constants

		private bool isInitialized = false;

		#endregion

	}
}