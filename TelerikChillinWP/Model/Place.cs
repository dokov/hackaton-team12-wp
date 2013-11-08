using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Telerik.Everlive.Sdk.Core.Model.Base;
using Telerik.Everlive.Sdk.Core.Model.System;
using Telerik.Everlive.Sdk.Core.Serialization;
using Telerik.Everlive.Sdk.Sample.WindowsPhone.Helpers;

namespace Telerik.Everlive.Sdk.Sample.WindowsPhone.Model
{
	public class Place : DataItem
	{

		#region Properties

		/// <summary>
		/// The text for the activity.
		/// </summary>
		public string Title { get; set; }

		public string Description { get; set; }

		public string Discount { get; set; }

		public string Address { get; set; }

		public GeoPoint Location { get; set; }

		public List<Guid> Images
		{
			get;
			set;
		}

		public int CommentsCount { get; set; }

		public int RatingsCount { get; set; }

		public double Rating { get; set; }

		#endregion

		#region ViewModel Properties

		[ServerIgnore]
		public string CreatedDate
		{
			get
			{
				return UIHelper.GetDateCreatedString(this.CreatedAt);
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

	}
}
