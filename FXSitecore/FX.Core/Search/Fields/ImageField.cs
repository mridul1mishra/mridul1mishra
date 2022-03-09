namespace FX.Core.Search.Fields
{
	public class ImageField
	{
		public string Url { get; set; }
		public string Alt { get; set; }

		public bool IsValid
		{
			get { return !string.IsNullOrEmpty(Url); }
		}
	}
}
