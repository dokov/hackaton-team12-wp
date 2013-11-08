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
using Telerik.Everlive.Sdk.Core.Model.Result;
using Telerik.Everlive.Sdk.Core.Query.Definition.Updating;
using Telerik.Everlive.Sdk.Core.Result;
using Telerik.Everlive.Sdk.Sample.WindowsPhone.Model;

namespace Telerik.Everlive.Sdk.Sample.WindowsPhone.UI
{
	public partial class CreateOrEditCommentPage : PhoneApplicationPage
	{
		public CreateOrEditCommentPage()
		{
			InitializeComponent();
		}

		private void ShareActivityButton_Click(object sender, EventArgs e)
		{
			var comment = this.currentComment;
			bool isNew = comment.Id == Guid.Empty;
			comment.Text = this.CommentTextBox.Text;
			comment.UserId = AppManager.Instance.LoggedUser.Id;

			if (isNew)
			{
				AppManager.Instance.Comments().Create(comment).TryExecute(this.OnCreateResult);
			}
			else
			{
				//TODO: Make this automatic (without having to manually create UpdateObject)
				var updatedFields = new List<UpdatedField>();
				updatedFields.Add(new UpdatedField("Comment", UpdateModifier.Set, comment.Text));
				UpdateObject updateObject = new UpdateObject(updatedFields);

				AppManager.Instance.Comments().Update(comment.Id, updateObject).TryExecute(this.OnUpdateResult);
			}
		}

		private void OnCreateResult(RequestResult<CreateResultItem> result)
		{
			if (result.Success)
			{
				this.currentComment.Id = result.Value.Id;
				AppManager.Instance.MustRefreshComments = true;
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
			if (result.Success)
			{
				AppManager.Instance.MustRefreshComments = true;
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

			var comment = (Comment)NavigationContext.GetData();
			if (comment != null)
			{
				this.currentComment = comment;
				if (comment.Text != null) this.CommentTextBox.Text = comment.Text;
				this.RefreshAppBar();
			}
		}

		private void UsernameTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.RefreshAppBar();
		}

		private void RefreshAppBar()
		{
			if (string.IsNullOrWhiteSpace(this.CommentTextBox.Text))
			{
				((ApplicationBarIconButton)this.ApplicationBar.Buttons[0]).IsEnabled = false;
			}
			else
			{
				((ApplicationBarIconButton)this.ApplicationBar.Buttons[0]).IsEnabled = true;
			}
		}


		#region Private Fields and Constants

		private Comment currentComment;

		#endregion
	}
}