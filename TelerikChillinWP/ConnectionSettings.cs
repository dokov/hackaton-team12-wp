using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Telerik.Everlive.Sdk.Sample.WindowsPhone
{
	public static class ConnectionSettings
	{
		/// <summary>
		/// Input your API key below to connect to your own app.
		/// </summary>
		public static readonly string EverliveApiKey = "E9ksyDFaxbPWtyZV";

		public static void ThrowError()
		{
			throw new Exception(
				"Please fill in your API key above and restart the app."
			);
		}

	}
}
