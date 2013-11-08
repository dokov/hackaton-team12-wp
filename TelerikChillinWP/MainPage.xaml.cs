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

namespace Telerik.Everlive.Sdk.Sample.WindowsPhone
{
	public partial class MainPage : PhoneApplicationPage
	{

		#region Constructors and Initialization

		// Constructor
		public MainPage()
		{
			InitializeComponent();
		}

		#endregion

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if (string.IsNullOrWhiteSpace(ConnectionSettings.EverliveApiKey) || ConnectionSettings.EverliveApiKey == "your-api-key-here")
			{
				MessageBox.Show("Hi there!\n\nBefore you can use this demo, you must insert your API key in the code.\n\nPlease go to ConnectionSettings.cs and put the API key for your Everlive Friends application.", "API Key needed", MessageBoxButton.OK);
				ConnectionSettings.ThrowError();
			}
			else
			{
				this.NavigationService.Navigate(new Uri("/UI/LoginPage.xaml", UriKind.Relative));
			}
		}

	}
}