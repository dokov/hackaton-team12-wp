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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telerik.Everlive.Sdk.Core.Model.System;
using Telerik.Everlive.Sdk.Core.Serialization;

namespace Telerik.Everlive.Sdk.Sample.WindowsPhone.Model
{
	public class CustomUser : User
	{

		#region Properties

		private string displayName;
		public string DisplayName
		{
			get
			{
				return this.displayName;
			}
			set
			{
				this.SetProperty(ref this.displayName, value, "DisplayName");
			}
		}

		private DateTime birthDate;
		public DateTime BirthDate
		{
			get
			{
				return this.birthDate;
			}
			set
			{
				this.SetProperty(ref this.birthDate, value, "BirthDate");
				this.OnPropertyChanged(new PropertyChangedEventArgs("AgeString"));
				this.OnPropertyChanged(new PropertyChangedEventArgs("BirthDateString"));
			}
		}

		private GenderEnum gender;
		public GenderEnum Gender
		{
			get
			{
				return this.gender;
			}
			set
			{
				this.SetProperty(ref this.gender, value, "Gender");
				this.OnPropertyChanged(new PropertyChangedEventArgs("GenderString"));
			}
		}

		private string about;
		public string About
		{
			get
			{
				return this.about;
			}
			set
			{
				this.SetProperty(ref this.about, value, "About");
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
				this.OnPropertyChanged(new PropertyChangedEventArgs("ProfilePicture"));
			}
		}

		#endregion

		#region ViewModel Properties

		[ServerIgnore]
		public int Age
		{
			get
			{
				if (this.BirthDate.Year > 1)
				{
					var now = DateTime.Now;
					var age = now.Year - this.BirthDate.Year;
					if (now.DayOfYear < this.BirthDate.DayOfYear) age--;
					return age;
				}
				else
				{
					return -1;
				}
			}
		}

		[ServerIgnore]
		public string AgeString
		{
			get
			{
				var age = this.Age;
				if (age >= 0)
				{
					return age + " years old";
				}
				else
				{
					return "age unknown";
				}
			}
		}

		[ServerIgnore]
		public string GenderString
		{
			get
			{
				return this.Gender.ToString().ToLowerInvariant();
			}
		}

		[ServerIgnore]
		public string BirthDateString
		{
			get
			{
				if (this.BirthDate.Year > 1)
				{
					return this.BirthDate.ToString("MMMM dd, yyyy");
				}
				else
				{
					return "not specified";
				}
			}
		}

		[ServerIgnore]
		public ImageSource ProfilePicture
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

		private ImageSource picture;

		#endregion
	}
}
