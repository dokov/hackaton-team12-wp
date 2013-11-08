using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Everlive.Sdk.Core.Model.Base;
using Telerik.Everlive.Sdk.Core.Serialization;

namespace Telerik.Everlive.Sdk.Sample.WindowsPhone.Model
{
	[ServerType("Menu")]
	public class CafeteriaMenu : DataItem
	{

		#region Properties

		private string content;
		public string Content
		{
			get
			{
				return this.content;
			}
			set
			{
				base.SetProperty(ref content, value, "Content");
			}
		}

		private DateTime menuDate;
		public DateTime MenuDate
		{
			get
			{
				return this.menuDate;
			}
			set
			{
				base.SetProperty(ref menuDate, value, "MenuDate");
			}
		}

		#endregion
	}
}
