using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Telerik.Everlive.Sdk.Core.Model.Result;
using Telerik.Everlive.Sdk.Core.Query.Definition.FormData;
using Telerik.Everlive.Sdk.Core.Result;
using Telerik.Everlive.Sdk.Sample.WindowsPhone.Model;

namespace Telerik.Everlive.Sdk.Sample.WindowsPhone.UI
{
	public partial class CreateActivityPage : PhoneApplicationPage
	{

		private string imageFilename;
		private Stream imageStream;
		private string imageContentType;
		private bool imageChanged = false;

		public CreateActivityPage()
		{
			InitializeComponent();
		}

		private void StoreActivity()
		{
			var activity = this.currentActivity;
			activity.Text = this.ActivityTextBox.Text;
			activity.UserId = AppManager.Instance.LoggedUser.Id;

			bool isNew = activity.Id == Guid.Empty;

			if (isNew)
			{
				AppManager.Instance.ShowProgress("sharing activity...");
			}
			else
			{
				AppManager.Instance.ShowProgress("saving changes...");
			}

			if (this.imageChanged && this.imageStream != null)
			{
				this.UploadActivityImage(delegate(RequestResult<CreateResultItem> result)
				{
					if (result.Success)
					{
						AppManager.Instance.InvokeInUIThread(delegate()
						{
							activity.PictureFileId = result.Value.Id;
							this.CreateOrUpdateActivity(activity);
						});
					}
					else
					{
						AppManager.Instance.HideProgress();
						AppManager.Instance.ShowErrorMessage(result.Error.Message);
					}
				});
			}
			else
			{
				this.CreateOrUpdateActivity(activity);
			}
		}

		private void UploadActivityImage(Action<RequestResult<CreateResultItem>> result)
		{
			var fileField = new FileField("file", this.imageFilename, this.imageContentType, this.imageStream);
			AppManager.Instance.Files().Upload(fileField).TryExecute(result);
		}

		private void CreateOrUpdateActivity(Activity activity)
		{
			bool isNew = activity.Id == Guid.Empty;
			if (isNew)
			{
				AppManager.Instance.Activities().Create(activity).TryExecute(this.OnCreateResult);
			}
			else
			{
				AppManager.Instance.Activities().Update(activity.Id, activity).TryExecute(this.OnUpdateResult);
			}
		}

		private void ShareActivityButton_Click(object sender, EventArgs e)
		{
			this.StoreActivity();
		}

		private void OnCreateResult(RequestResult<CreateResultItem> result)
		{
			AppManager.Instance.HideProgress();
			if (result.Success)
			{
				this.currentActivity.Id = result.Value.Id;
				AppManager.Instance.MustRefreshActivities = true;
				AppManager.Instance.InvokeInUIThread(delegate()
				{
					this.NavigationService.GoBack();
				});
			}
			else
			{
				AppManager.Instance.ShowErrorMessage(result.Error.Message);
			}
		}

		private void OnUpdateResult(RequestResult<bool> result)
		{
			AppManager.Instance.HideProgress();
			if (result.Success)
			{
				AppManager.Instance.MustRefreshActivities = true;
				AppManager.Instance.InvokeInUIThread(delegate()
				{
					this.NavigationService.GoBack();
				});
			}
			else
			{
				AppManager.Instance.ShowErrorMessage(result.Error.Message);
			}
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			var activity = (Activity)NavigationContext.GetData();
			if (activity != null)
			{
				this.currentActivity = activity;
				if (activity.Id != Guid.Empty) {
					this.PageTitle.Text = "edit activity";

					if (activity.Text != null) this.ActivityTextBox.Text = activity.Text;
					if (activity.PictureFileId != Guid.Empty)
					{
						this.ActivityImage.Source = activity.ActivityImage;
						this.ActivityImage.Visibility = Visibility.Visible;
					}
				}
				this.RefreshAppBar();
			}
		}

		private void UsernameTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.RefreshAppBar();
		}

		private void RefreshAppBar()
		{
			if (string.IsNullOrWhiteSpace(this.ActivityTextBox.Text))
			{
				((ApplicationBarIconButton)this.ApplicationBar.Buttons[0]).IsEnabled = false;
			}
			else
			{
				((ApplicationBarIconButton)this.ApplicationBar.Buttons[0]).IsEnabled = true;
			}
		}


		#region Private Fields and Constants

		private Activity currentActivity;

		#endregion

		private void Border_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			PhotoChooserTask photo = new PhotoChooserTask();
			photo.Completed += new EventHandler<PhotoResult>(photo_Completed);
			photo.ShowCamera = true;
			photo.PixelWidth = 500;
			photo.PixelHeight = 400;
			photo.Show();
		}

		void photo_Completed(object sender, PhotoResult e)
		{
			if (e.TaskResult == TaskResult.OK)
			{
				BitmapImage img = new BitmapImage();
				img.SetSource(e.ChosenPhoto);
				this.ActivityImage.Source = img;
				this.NoImageTextBox.Visibility = Visibility.Collapsed;

				e.ChosenPhoto.Position = 0;
				this.imageStream = e.ChosenPhoto;
				this.imageFilename = e.OriginalFileName;
				if (imageFilename.EndsWith(".jpg"))
				{
					imageContentType = "image/jpg";
				}
				else if (imageFilename.EndsWith(".png"))
				{
					imageContentType = "image/png";
				}
				else
				{
					imageContentType = "application/octet-stream";
				}

				this.imageChanged = true;
			}
		}
	}
}