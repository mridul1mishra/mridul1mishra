using FX.Core.Models.Base;

namespace FX.Core.Models.Components.TitleAndText
{
	public interface ITitleAndTextItem : ISitecoreItem
	{
		string Title { get; set; }
		string Text { get; set; }
	}
}
