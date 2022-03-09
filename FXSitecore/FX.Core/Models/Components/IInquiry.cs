using FX.Core.Models.Base;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Models.Components 
{
    public interface IInquiry : IBasePageComponentFields
    {
        string Title { get; set; }
        Link Link { get; set; }
        string Description { get; set; }
    }
}
