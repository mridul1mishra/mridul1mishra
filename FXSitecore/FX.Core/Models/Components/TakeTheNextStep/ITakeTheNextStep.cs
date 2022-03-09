using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Components.TakeTheNextStep
{
	public interface ITakeTheNextStep : IBasePageComponentFields
    {
		[SitecoreChildren(InferType = true)]
		IEnumerable<INextStepFields> NextSteps { get; set; }
	}
}
