using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Everlive.Sdk.Core.Model.Base;
using Telerik.Everlive.Sdk.Core.Serialization;
using Telerik.Everlive.Sdk.Sample.WindowsPhone.Helpers;

namespace Telerik.Everlive.Sdk.Sample.WindowsPhone.Model
{
	public class Comment : DataItem
	{

		#region Properties

		/// <summary>
		/// The text for the comment.
		/// </summary>
		private string text;
		[ServerProperty("Comment")]
		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.SetProperty(ref this.text, value, "Text");
			}
		}

		/// <summary>
		/// The ID of the user who created the comment.
		/// </summary>
		private Guid userId;
		public Guid UserId
		{
			get
			{
				return this.userId;
			}
			set
			{
				this.SetProperty(ref this.userId, value, "UserId");
			}
		}

		/// <summary>
		/// The ID of the activity this comment is for.
		/// </summary>
		private Guid activityId;
		public Guid ActivityId
		{
			get
			{
				return this.activityId;
			}
			set
			{
				this.SetProperty(ref this.activityId, value, "ActivityId");
			}
		}

		#endregion

		#region ViewModel Properties

		[ServerIgnore]
		public string AuthorName
		{
			get
			{
				if (this.userDisplayName == null)
				{
					AppManager.Instance.GetUserDisplayName(
						this.UserId,
						delegate(string displayName)
						{
							AppManager.Instance.InvokeInUIThread(() => this.AuthorName = displayName);
						}
					);
				}
				return this.userDisplayName;
			}
			protected set
			{
				this.SetProperty(ref this.userDisplayName, value, "AuthorName");
			}
		}

		[ServerIgnore]
		public string CreatedDate
		{
			get
			{
				return UIHelper.GetDateCreatedString(this.CreatedAt);
			}
		}

		#endregion

		#region Private Fields and Constants

		private string userDisplayName = null;

		#endregion
	}
}
