using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Telerik.Everlive.Sdk.Core.Model.Base;
using Telerik.Everlive.Sdk.Core.Query.Definition.Filtering;
using Telerik.Everlive.Sdk.Core.Query.Definition.Filtering.Simple;
using Telerik.Everlive.Sdk.Core.Result;
using Telerik.Everlive.Sdk.Core.Serialization;
using Telerik.Everlive.Sdk.Sample.WindowsPhone.Helpers;

namespace Telerik.Everlive.Sdk.Sample.WindowsPhone.Model
{
	public class Activity : DataItem
	{

		#region Properties

		/// <summary>
		/// The text for the activity.
		/// </summary>
		private string text;
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
		/// The ID of the user who created(shared) the activity.
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
		/// 
		/// </summary>
		private Guid pictureFileId;
		[ServerProperty("Picture")]
		public Guid PictureFileId
		{
			get
			{
				return this.pictureFileId;
			}
			set
			{
				this.SetProperty(ref this.pictureFileId, value, "PictureFileId");
				this.OnPropertyChanged(new PropertyChangedEventArgs("ActivityImage"));
			}
		}

		/// <summary>
		/// Contains the IDs of the users who liked the activity.
		/// </summary>
		private List<Guid> likes;
		public List<Guid> Likes
		{
			get
			{
				return this.likes;
			}
			set
			{
				this.SetProperty(ref this.likes, value, "Likes");
				this.OnPropertyChanged(new PropertyChangedEventArgs("LikesCount"));
				this.OnPropertyChanged(new PropertyChangedEventArgs("LikesCountString"));
				this.OnPropertyChanged(new PropertyChangedEventArgs("LikesAndCommentsCount"));
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
				this.userDisplayName = value;
				this.OnPropertyChanged(new PropertyChangedEventArgs("AuthorName"));
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

		[ServerIgnore]
		public int LikesCount
		{
			get
			{
				int count = 0;
				if (this.Likes != null)
				{
					count = this.Likes.Count;
				}

				return count;
			}
		}

		[ServerIgnore]
		public string LikesCountString
		{
			get
			{
				int count = this.LikesCount;
				if (count == 1)
				{
					return "1 like";
				}
				else
				{
					return String.Format("{0} likes", count);
				}
			}
		}

		[ServerIgnore]
		public int CommentsCount
		{
			get
			{
				if (!this.commentsCount.HasValue)
				{
					ValueCondition c = new ValueCondition("ActivityId", ValueConditionOperator.ValueEquals, this.Id);
					FilteringDefinition filter = new FilteringDefinition(c);
					AppManager.Instance.Comments().GetCount().Where(c).TryExecute(
						delegate(RequestResult<int> result)
						{
							if (result.Success)
							{
								AppManager.Instance.InvokeInUIThread(() => this.CommentsCount = result.Value);
							}
						}
					);
					return 0;
				}
				else
				{
					return this.commentsCount.Value;
				}
			}
			set
			{
				this.commentsCount = value;
				this.OnPropertyChanged(new PropertyChangedEventArgs("CommentsCount"));
				this.OnPropertyChanged(new PropertyChangedEventArgs("CommentsCountString"));
				this.OnPropertyChanged(new PropertyChangedEventArgs("LikesAndCommentsCount"));
			}
		}

		[ServerIgnore]
		public string CommentsCountString
		{
			get
			{
				var count = this.CommentsCount;

				if (count == 1)
				{
					return "1 comment";
				}
				else
				{
					return String.Format("{0} comments", count);
				}
			}
		}

		[ServerIgnore]
		public string LikesAndCommentsCount
		{
			get
			{
				return this.LikesCountString + ", " + this.CommentsCountString;
			}
		}

		[ServerIgnore]
		public ImageSource ActivityImage
		{
			get
			{
				if (this.PictureFileId != Guid.Empty)
				{
					if (this.picture == null)
					{
						string profilePictureUrl = AppManager.Instance.Files().GetFileDownloadUrl(this.PictureFileId);
						this.picture = new BitmapImage(new Uri(profilePictureUrl, UriKind.Absolute));
					}
					return this.picture;
				}
				else
				{
					return null;
				}
			}
		}

		#endregion

		#region Private Fields and Constants

		private string userDisplayName = null;
		private int? commentsCount;
		private ImageSource picture;

		#endregion

	}
}
