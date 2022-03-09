using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Base
{
	public interface INextStepFields : ISitecoreItem
	{
		Image Image { get; set; }
		string Text { get; set; }
	}
}
