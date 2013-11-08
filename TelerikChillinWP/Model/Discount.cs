using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Telerik.Everlive.Sdk.Core.Model.Base;
using Telerik.Everlive.Sdk.Core.Model.System;
using Telerik.Everlive.Sdk.Core.Query.Definition.Filtering;
using Telerik.Everlive.Sdk.Core.Query.Definition.Filtering.Array;
using Telerik.Everlive.Sdk.Core.Query.Definition.Filtering.Simple;
using Telerik.Everlive.Sdk.Core.Result;
using Telerik.Everlive.Sdk.Core.Serialization;
using Telerik.Everlive.Sdk.Sample.WindowsPhone.Helpers;
using Telerik.Everlive.Sdk.Sample.WindowsPhone.UI.ViewModel;
using System.Linq;
using System.Collections;

namespace Telerik.Everlive.Sdk.Sample.WindowsPhone.Model
{
	[ServerType("Discounts")]
	public class Discount : DataItem
	{

		#region Properties

		/// <summary>
		/// The text for the activity.
		/// </summary>
		private string title;
		public string Title
		{
			get
			{
				return this.title;
			}
			set
			{
				this.SetProperty(ref this.title, value, "Title");
			}
		}

		private string address;
		public string Address
		{
			get
			{
				return this.address;
			}
			set
			{
				this.SetProperty(ref this.address, value, "Address");
			}
		}

		private string info;
		public string Info
		{
			get
			{
				return this.info;
			}
			set
			{
				this.SetProperty(ref this.info, value, "Info");
			}
		}

		private string link;
		public string Link
		{
			get
			{
				return this.link;
			}
			set
			{
				this.SetProperty(ref this.link, value, "Link");
			}
		}


		private string discount;
		[ServerProperty("Discount")]
		public string DiscountText
		{
			get
			{
				return this.discount;
			}
			set
			{
				this.SetProperty(ref this.discount, value, "Discount");
			}
		}


		private string condition;
		public string Condition
		{
			get
			{
				return this.condition;
			}
			set
			{
				this.SetProperty(ref this.condition, value, "Condition");
			}
		}

		private string category;
		public string Category
		{
			get
			{
				return this.category;
			}
			set
			{
				this.SetProperty(ref this.category, value, "Category");
			}
		}
		

		/// <summary>
		/// 
		/// </summary>
		private List<Guid> fileIds;
		[ServerProperty("InfoFiles")]
		public List<Guid> FileIds
		{
			get
			{
				return this.fileIds;
			}
			set
			{
				this.SetProperty(ref this.fileIds, value, "FileIds");
			}
		}

		#endregion

		#region ViewModel Properties

		[ServerIgnore]
		public string AddedDate
		{
			get
			{
				return "added on " + this.CreatedAt.ToString("MMMM dd, yyyy");
			}
		}

		[ServerIgnore]
		public string LastUpdatedDate
		{
			get
			{
				return this.ModifiedAt.ToString("MMMM dd, yyyy");
			}
		}

		[ServerIgnore]
		public Uri LinkUri
		{
			get
			{
				return this.Link == null ? null : new Uri(this.Link, UriKind.Absolute);
			}
		}

		[ServerIgnore]
		public List<FileItem> FileItems
		{
			get
			{
				return this.fileItems;
			}
		}

		public Visibility WebSiteVisibility
		{
			get
			{
				return this.Link == null ? Visibility.Collapsed : Visibility.Visible;
			}
		}

		public Visibility InfoVisibility
		{
			get
			{
				return this.Info == null ? Visibility.Collapsed : Visibility.Visible;
			}
		}

		public Visibility FilesVisibility
		{
			get
			{
				return this.FileIds == null || this.FileIds.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
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

		#endregion

		public void LoadFiles()
		{
			if (this.FileIds == null)
			{
				return;
			}

			var fileIds = new List<object>();
			this.FileIds.ForEach(fid => fileIds.Add(fid));
			ArrayCondition vc = new ArrayCondition("Id", ArrayConditionOperator.ValueIsIn, fileIds);
			AppManager.Instance.Files().GetByFilter().Where(vc).TryExecute(
				delegate(RequestResult<IEnumerable<File>> result)
				{
					AppManager.Instance.InvokeInUIThread(
						delegate()
						{
							AppManager.Instance.HideProgress();
							if (result.Success)
							{
								List<FileItem> fileItems = new List<FileItem>();
								foreach (var file in result.Value)
								{
									fileItems.Add(new FileItem() { Name = file.Filename, Uri = new Uri(AppManager.Instance.Files().GetFileDownloadUrl(file.Id), UriKind.Absolute) });
								}

								this.fileItems = fileItems;
								this.OnPropertyChanged(new PropertyChangedEventArgs("FileItems"));
							}
							else
							{
								AppManager.Instance.ShowErrorMessage(result.Error.Message);
							}
						}
					);
				}
			);
		}

		#region Private Fields and Constants

		private int? commentsCount;
		private List<FileItem> fileItems;

		#endregion

	}
}
