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
using Telerik.Everlive.Sdk.Core.Linq.Translators;
using Telerik.Everlive.Sdk.Core.Query.Definition.Filtering;
using Telerik.Everlive.Sdk.Core.Query.Definition.Filtering.Simple;
using Telerik.Everlive.Sdk.Core.Query.Definition.Sorting;
using Telerik.Everlive.Sdk.Core.Query.Definition.Updating;
using Telerik.Everlive.Sdk.Core.Result;
using Telerik.Everlive.Sdk.Sample.WindowsPhone.Model;

namespace Telerik.Everlive.Sdk.Sample.WindowsPhone.UI
{
	public partial class CafeteriaPage : PhoneApplicationPage
	{

		public CafeteriaPage()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
			{
				var now = DateTime.UtcNow;
				var dayOfWeek = (int)now.DayOfWeek;
				if (dayOfWeek < 3)
				{
					this.MenuPivot.Items.Remove(this.NextWeekPivotItem);
				}
				else if (dayOfWeek == 3)
				{
					this.MenuPivot.Items.Remove(this.ThisWeekPivotItem);
				}
				else if (dayOfWeek == 4)
				{
					this.MenuPivot.Items.Remove(this.ThisWeekPivotItem);
					this.MenuPivot.Items.Remove(this.TomorrowPivotItem);
				}
				else
				{
					this.MenuPivot.Items.Remove(this.ThisWeekPivotItem);
					this.MenuPivot.Items.Remove(this.TomorrowPivotItem);
					//this.MenuPivot.Items.Remove(this.TodayPivotItem);
				}
			}
		}

		private void GetTodayMenu()
		{
			var now = DateTime.UtcNow;
			var today = now.Date;
			var tomorrow = today.AddDays(1);
			RefreshMenu(today, tomorrow, this.TodayMenuText, this.NoMenuTodayText);
		}

		private void GetTomorrowMenu()
		{
			var now = DateTime.UtcNow;
			var today = now.Date;
			var tomorrow = today.AddDays(1);
			var theDayAfter = tomorrow.AddDays(1);
			RefreshMenu(tomorrow, theDayAfter, this.TomorrowMenuText, this.NoMenuTomorrowText);
		}

		private void GetThisWeekMenu()
		{
			var now = DateTime.UtcNow;
			var today = now.Date;
			var dayOfWeek = (int)now.DayOfWeek;
			var endOfWeek = today.AddDays(7-dayOfWeek);
			RefreshMenu(today, endOfWeek, this.ThisWeekMenuText, this.NoMenuThisWeekText);
		}

		private void GetNextWeekMenu()
		{
			var now = DateTime.UtcNow;
			var today = now.Date;
			var dayOfWeek = (int)now.DayOfWeek;
			var endOfWeek = today.AddDays(7 - dayOfWeek);
			var endOfNextWeek = endOfWeek.AddDays(7);
			RefreshMenu(endOfWeek, endOfNextWeek, this.NextWeekMenuText, this.NoMenuNextWeekText);
		}

		private void RefreshMenu(DateTime fromDate, DateTime toDate, TextBlock target, TextBlock noMenuText)
		{
			AppManager.Instance.CafeteriaMenu().Get().Where(c => c.MenuDate >= fromDate && c.MenuDate < toDate).TryExecute(
				delegate(RequestResult<IEnumerable<CafeteriaMenu>> result)
				{
					if (result.Success && result.Value.Count() > 0)
					{
						AppManager.Instance.InvokeInUIThread(delegate()
						{
							target.Visibility = Visibility.Visible;
							noMenuText.Visibility = Visibility.Collapsed;

							if (result.Value.Count() == 1)
							{
								var menu = result.Value.First();
								target.Text = menu.Content;
							}
							else
							{
								target.Inlines.Clear();
								foreach (var cm in result.Value)
								{
									target.Inlines.Add(new Run() { FontWeight = FontWeights.Bold, Text = cm.MenuDate.ToString("") + Environment.NewLine + Environment.NewLine });
									target.Inlines.Add(new Run() { FontWeight = FontWeights.Normal, Text = cm.Content + Environment.NewLine + Environment.NewLine });
								}
							}

						});
					}
					else
					{
						AppManager.Instance.InvokeInUIThread(delegate()
						{
							target.Visibility = Visibility.Collapsed;
							noMenuText.Visibility = Visibility.Visible;
						});
					}
				}
			);
		}

		private void MenuPivot_LoadingPivotItem(object sender, PivotItemEventArgs e)
		{
			if (e.Item == this.TodayPivotItem) 
			{
				this.GetTodayMenu();
			}
			else if (e.Item == this.TomorrowPivotItem)
			{
				this.GetTomorrowMenu();
			}
			else if (e.Item == this.ThisWeekPivotItem)
			{
				this.GetThisWeekMenu();
			}
			else if (e.Item == this.NextWeekPivotItem)
			{
				this.GetNextWeekMenu();
			}
		}
	}
}