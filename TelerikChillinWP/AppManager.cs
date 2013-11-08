using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Phone.Shell;
using Telerik.Everlive.Sdk.Core;
using Telerik.Everlive.Sdk.Core.Handlers.Common;
using Telerik.Everlive.Sdk.Core.Handlers.Data;
using Telerik.Everlive.Sdk.Core.Managers;
using Telerik.Everlive.Sdk.Core.Result;
using Telerik.Everlive.Sdk.Sample.WindowsPhone.Model;

namespace Telerik.Everlive.Sdk.Sample.WindowsPhone
{
	public class AppManager
	{

		#region Properties

		public EverliveApp MyApp { get; set; }

		public CustomUser LoggedUser { get; set; }

		public bool IsLoggedIn { get; set; }

		public bool MustRefreshActivities { get; set; }

		public bool MustRefreshComments { get; set; }

		public bool MustRefreshUsers { get; set; }

		private static AppManager instance;
		public static AppManager Instance
		{
			get
			{
				if (AppManager.instance == null)
				{
					AppManager.instance = new AppManager();
					AppManager.instance.Initialize();
				}

				return AppManager.instance;
			}
		}

		#endregion

		#region Constructors and Initialization

		private AppManager()
		{

		}

		private void Initialize()
		{
			EverliveApp myApp = new EverliveApp(new EverliveAppSettings()
			{
				ApiKey = ConnectionSettings.EverliveApiKey,
				UseHttps = false
			});
			this.MyApp = myApp;
		}

		#endregion

		#region Public Methods

		public void ShowErrorMessage(string errorMessage)
		{
			Deployment.Current.Dispatcher.BeginInvoke(() =>
			{
				var pi = SystemTray.ProgressIndicator;
				if (pi != null) pi.IsVisible = false;
				MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK);
			});
		}

		public void ShowProgress(string message)
		{
			Deployment.Current.Dispatcher.BeginInvoke(() =>
			{
				var pi = new ProgressIndicator();
				pi.IsIndeterminate = true;
				pi.Text = message;
				pi.IsVisible = true;
				SystemTray.ProgressIndicator = pi;
			});
		}

		public void HideProgress()
		{
			Deployment.Current.Dispatcher.BeginInvoke(() =>
			{
				var pi = SystemTray.ProgressIndicator;
				if (pi != null) pi.IsVisible = false;
			});
		}

		public void InvokeInUIThread(Action a)
		{
			Deployment.Current.Dispatcher.BeginInvoke(() =>
			{
				a.Invoke();
			});
		}

		public void GetUserDisplayName(Guid userId, Action<string> callback)
		{
			this.GetUser(userId, delegate(CustomUser user)
			{
				if (user != null)
				{
					callback(user.DisplayName);
				}
				else
				{
					callback("[user not found]");
				}
			});
		}

		public void GetUser(Guid userId, Action<CustomUser> callback)
		{
			CustomUser user;
			if (this.usersCache.TryGetValue(userId, out user))
			{
				callback(user);
			}
			else 
			{
				this.Users().GetById(userId).TryExecute(delegate(RequestResult<CustomUser> result)
				{
					if (result.Success)
					{
						this.usersCache[userId] = result.Value;
						callback(result.Value);
					}
					else
					{
						callback(null);
					}
				});
			}
		}

		public AuthenticationManagerRaw Authentication()
		{
			return this.MyApp.Authentication().Raw;
		}

		public DataHandler<Activity> Activities()
		{
			return this.MyApp.WorkWith().Data<Activity>(ActiviesTypeName);
		}

		public DataHandler<Comment> Comments()
		{
			return this.MyApp.WorkWith().Data<Comment>(CommentsTypeName);
		}

		public UsersHandler<CustomUser> Users()
		{
			return this.MyApp.WorkWith().Users<CustomUser>();
		}

		public FilesHandler Files()
		{
			return this.MyApp.WorkWith().Files();
		}


		public DataHandler<CafeteriaMenu> CafeteriaMenu()
		{
			return this.MyApp.WorkWith().Data<CafeteriaMenu>();
		}

		public DataHandler<Discount> Discounts()
		{
			return this.MyApp.WorkWith().Data<Discount>();
		}

		public DataHandler<Place> Places()
		{
			return this.MyApp.WorkWith().Data<Place>();
		}



		#endregion

		#region Private Fields and Constants

		internal const string ActiviesTypeName = "Activities";
		internal const string CommentsTypeName = "Comments";

		private Dictionary<Guid, CustomUser> usersCache = new Dictionary<Guid, CustomUser>();

		#endregion

	}
}
