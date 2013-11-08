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
using Telerik.Everlive.Sdk.Core.Query.Definition.Updating;
using Telerik.Everlive.Sdk.Core.Result;
using Telerik.Everlive.Sdk.Sample.WindowsPhone.Model;

namespace Telerik.Everlive.Sdk.Sample.WindowsPhone.UI
{
	public partial class ViewActivityPage : PhoneApplicationPage
	{

		public ViewActivityPage()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
			{
				var activity = (Activity)NavigationContext.GetData();
				if (activity != null)
				{
					this.DataContext = activity;
					this.currentActivity = activity;
					this.RefreshApplicationBar();

					if (activity.Id != Guid.Empty)
					{
						this.ReloadComments();
					}
				}
			}
			else
			{
				if (AppManager.Instance.MustRefreshComments)
				{
					AppManager.Instance.MustRefreshComments = false;
					this.ReloadComments();
				}
			}
		}

		private void ReloadComments()
		{
			AppManager.Instance.Comments().Get().Where(c => c.ActivityId == this.currentActivity.Id).OrderByDescending(c => c.CreatedAt).TryExecute(
				delegate(RequestResult<IEnumerable<Comment>> result)
				{
					if (result.Success)
					{
						AppManager.Instance.InvokeInUIThread(delegate()
						{
							this.currentActivity.CommentsCount = result.Value.Count();
							this.CommentsList.ItemsSource = result.Value;
						});
					}
					else
					{
						AppManager.Instance.ShowErrorMessage(result.Error.Message);
					}
				}
			);
		}

		private void RefreshApplicationBar()
		{
			var appBar =  this.ApplicationBar;


			var likeButton = new ApplicationBarIconButton() {
				IconUri = new Uri("Images/appbar_like.png", UriKind.Relative),
				Text = "like"
			};
			likeButton.Click += new EventHandler(LikeButton_Click);

			var unlikeButton = new ApplicationBarIconButton()
			{
				IconUri = new Uri("Images/appbar_unlike.png", UriKind.Relative),
				Text = "unlike"
			};
			unlikeButton.Click += new EventHandler(UnlikeButton_Click);

			var commentButton = new ApplicationBarIconButton()
			{
				IconUri = new Uri("Images/appbar_comment.png", UriKind.Relative),
				Text = "comment"
			};
			commentButton.Click += new EventHandler(CommentButton_Click);

			var editButton = new ApplicationBarIconButton() {
				IconUri = new Uri("Images/appbar_edit.png", UriKind.Relative),
				Text = "edit"
			};
			editButton.Click += new EventHandler(EditButton_Click);

			var deleteButton = new ApplicationBarIconButton() {
				IconUri = new Uri("Images/appbar_delete.png", UriKind.Relative),
				Text = "delete"
			};
			deleteButton.Click += new EventHandler(DeleteButton_Click);

			bool isOwnPost = this.currentActivity.UserId == AppManager.Instance.LoggedUser.Id;
			bool isLikedByMe = this.currentActivity.Likes != null && this.currentActivity.Likes.Contains(AppManager.Instance.LoggedUser.Id);

			appBar.Buttons.Clear();

			if (isLikedByMe)
			{
				appBar.Buttons.Add(unlikeButton);
			}
			else
			{
				appBar.Buttons.Add(likeButton);
			}
			appBar.Buttons.Add(commentButton);
			if (isOwnPost)
			{
				appBar.Buttons.Add(editButton);
				appBar.Buttons.Add(deleteButton);
			}
		}

		private void UpdateLikes(List<Guid> likes)
		{
			UpdateObject updateObject = new UpdateObject(new List<UpdatedField>() { new UpdatedField("Likes", UpdateModifier.Set, likes) });
			AppManager.Instance.Activities().Update(this.currentActivity.Id, updateObject).TryExecute( 
				delegate(RequestResult<bool> result)
				{
					if (result.Success)
					{
						AppManager.Instance.InvokeInUIThread(delegate()
						{
							this.currentActivity.Likes = likes;
							this.RefreshApplicationBar();
						});
					}
					else
					{
						AppManager.Instance.ShowErrorMessage(result.Error.Message);
					}
				}
			);
		}

		private void OnLikesUpdated(UpdateResult result)
		{
			if (result.Success)
			{
				this.RefreshApplicationBar();
			}
			else
			{
				AppManager.Instance.ShowErrorMessage(result.Error.Message);
			}
		}

		private void CommentButton_Click(object sender, EventArgs e)
		{
			Comment newComment = new Comment();
			newComment.ActivityId = this.currentActivity.Id;
			NavigationService.Navigate("/UI/CreateOrEditCommentPage.xaml", newComment);
		}

		private void LikeButton_Click(object sender, EventArgs e)
		{
			var likesList = new List<Guid>();
			if (this.currentActivity.Likes != null) likesList.AddRange(this.currentActivity.Likes);
			likesList.Add(AppManager.Instance.LoggedUser.Id);
			this.UpdateLikes(likesList);
		}

		private void UnlikeButton_Click(object sender, EventArgs e)
		{
			var likesList = new List<Guid>();
			if (this.currentActivity.Likes != null) likesList.AddRange(this.currentActivity.Likes);
			likesList.Remove(AppManager.Instance.LoggedUser.Id);
			this.UpdateLikes(likesList);
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			NavigationService.Navigate("/UI/CreateOrEditActivityPage.xaml", this.currentActivity);
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			AppManager.Instance.InvokeInUIThread(delegate()
			{
				var dr = MessageBox.Show("Are you sure you want to delete this activity?", "Delete Activity", MessageBoxButton.OKCancel);
				if (dr == MessageBoxResult.OK)
				{
					AppManager.Instance.Activities().Delete(this.currentActivity.Id).TryExecute(
						delegate(RequestResult<bool> result)
						{
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
					);
				}
			});
		}

		#region Private Fields and Constants

		private Activity currentActivity;

		#endregion
	}
}