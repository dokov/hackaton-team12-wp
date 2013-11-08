using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
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

namespace Telerik.Everlive.Sdk.Sample.WindowsPhone
{
	public partial class CreateAccountPage : PhoneApplicationPage
	{
		private string imageFilename;
		private Stream imageStream;
		private string imageContentType;
		private bool imageChanged = false;

		public CreateAccountPage()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			var user = (CustomUser)NavigationContext.GetData();
			if (user != null)
			{
				this.currentUser = user;

				if (user.Id != Guid.Empty)
				{
					this.PageTitle.Text = "edit profile";

					//Load profile data
					this.UsernameTextBox.Text = user.Username;
					this.NameTextBox.Text = user.DisplayName;
					if (user.About != null) this.AboutTextBox.Text = user.About;
					this.BirthDatePicker.Value = user.BirthDate;
					this.GenderPicker.SelectedIndex = user.Gender == GenderEnum.Unspecified ? 0 : user.Gender == GenderEnum.Male ? 1 : 2;

					//Hide unnecessary fields
					this.PasswordTextBox.Visibility = Visibility.Collapsed;
					this.PasswordLabel.Visibility = Visibility.Collapsed;
					this.UsernameTextBox.Visibility = Visibility.Collapsed;
					this.UsernameLabel.Visibility = Visibility.Collapsed;

					if (user.PictureFileId != Guid.Empty)
					{
						this.ProfileImage.Source = user.ProfilePicture;
						this.NoImageTextBox.Visibility = Visibility.Collapsed;
					}
				}
				this.RefreshAppBar();
			}
		}

		private bool HasValidInput()
		{
			bool result = true;

			if (string.IsNullOrWhiteSpace(this.NameTextBox.Text)) result = false;

			if (string.IsNullOrWhiteSpace(this.UsernameTextBox.Text)) result = false;

			if (this.currentUser.Id == Guid.Empty && string.IsNullOrWhiteSpace(this.PasswordTextBox.Password)) result = false;

			return result;
		}

		private void RefreshAppBar()
		{
			if (this.HasValidInput())
			{
				((ApplicationBarIconButton)this.ApplicationBar.Buttons[0]).IsEnabled = true;
			}
			else
			{
				((ApplicationBarIconButton)this.ApplicationBar.Buttons[0]).IsEnabled = false;
			}
		}

		private void StoreUser()
		{
			var user = this.currentUser;
			user.DisplayName = this.NameTextBox.Text;
			user.Gender = (GenderEnum)Enum.Parse(typeof(GenderEnum), (string)((ListPickerItem)this.GenderPicker.SelectedItem).Content, true);
			user.BirthDate = this.BirthDatePicker.Value.Value;
			user.About = this.AboutTextBox.Text;
			
			bool isNew = user.Id == Guid.Empty;
			if (isNew)
			{
				user.Username = this.UsernameTextBox.Text;
				user.Password = this.PasswordTextBox.Password;
				AppManager.Instance.ShowProgress("creating account...");
			}
			else
			{
				AppManager.Instance.ShowProgress("saving changes...");
			}

			if (this.imageChanged && this.imageStream != null)
			{
				this.UploadProfilePicture(delegate(RequestResult<CreateResultItem> result)
				{
					if (result.Success)
					{
						AppManager.Instance.InvokeInUIThread(delegate()
						{
							user.PictureFileId = result.Value.Id;
							this.CreateOrUpdateUserAccount(user);
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
				this.CreateOrUpdateUserAccount(user);
			}
		}

		private void UploadProfilePicture(Action<RequestResult<CreateResultItem>> result)
		{
			var fileField = new FileField("file", this.imageFilename, this.imageContentType, this.imageStream);
			AppManager.Instance.Files().Upload(fileField).TryExecute(result);
		}

		private void CreateOrUpdateUserAccount(CustomUser user)
		{
			bool isNew = user.Id == Guid.Empty;
			if (isNew)
			{
				AppManager.Instance.Users().Create(user).TryExecute(this.OnCreateUserResult);
			}
			else
			{
				AppManager.Instance.Users().Update(user.Id, user).TryExecute(this.OnUpdateResult);
			}
		}

		private void OnCreateUserResult(RequestResult<CreateResultItem> result)
		{
			AppManager.Instance.HideProgress();

			this.Dispatcher.BeginInvoke(delegate()
			{
				if (result.Success)
				{
					MessageBox.Show("Your new account is now ready.", "Success!", MessageBoxButton.OK);
					this.NavigationService.RemoveBackEntry();
					this.NavigationService.Navigate(new Uri("/UI/LoginPage.xaml?Username=" + this.UsernameTextBox.Text + "&Password=" + this.PasswordTextBox.Password, UriKind.Relative));
				}
				else
				{
					AppManager.Instance.ShowErrorMessage(result.Error.Message);
				}
			});
		}

		private void OnUpdateResult(RequestResult<bool> result)
		{
			AppManager.Instance.HideProgress();

			if (result.Success)
			{
				AppManager.Instance.MustRefreshUsers = true;
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

		private void ApplicationBarIconButton_Click(object sender, EventArgs e)
		{
			this.StoreUser();
		}

		private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.RefreshAppBar();
		}

		private void UsernameTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.RefreshAppBar();
		}

		private void PasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
		{
			this.RefreshAppBar();
		}


		#region Private Fields and Constants

		private CustomUser currentUser;

		#endregion

		private void Border_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			PhotoChooserTask photo = new PhotoChooserTask();
			photo.Completed += new EventHandler<PhotoResult>(photo_Completed);
			photo.ShowCamera = true;
			photo.PixelWidth = 300;
			photo.PixelHeight = 300;
			photo.Show();
		}

		void photo_Completed(object sender, PhotoResult e)
		{
			if (e.TaskResult == TaskResult.OK)
			{
				BitmapImage img = new BitmapImage();
				img.SetSource(e.ChosenPhoto);
				this.ProfileImage.Source = img;
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