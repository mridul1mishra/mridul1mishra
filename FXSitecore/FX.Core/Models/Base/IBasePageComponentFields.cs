
namespace FX.Core.Models.Base
{
	public interface IBasePageComponentFields : ISitecoreItem
	{
		string SectionTitle { get; set; }
		bool ShowInStickyTab { get; set; }
        
    }
}
