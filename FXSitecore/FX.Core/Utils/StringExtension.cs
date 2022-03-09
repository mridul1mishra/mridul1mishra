using System;

namespace FX.Core.Utils
{
	public static class StringExtension
	{
		public static string Left(this string value, int maxLength)
		{
			if (string.IsNullOrEmpty(value))
				return value;
			maxLength = Math.Abs(maxLength);
			return (value.Length < maxLength) ? value : value.Substring(0, maxLength);
		}

		public static string FormatInBytes(this string value)
		{
			long bytes = 0;
			if (long.TryParse(value, out bytes))
			{
				const int scale = 1024;
				string[] orders = new string[] { "GB", "MB", "KB", "Bytes" };
				long max = (long)Math.Pow(scale, orders.Length - 1);
				foreach (string order in orders)
				{
					if (bytes > max)
					{
						return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);
					}
					max /= scale;
				}
				return "0 Bytes";
			}
			return value;
		}
	}
}
