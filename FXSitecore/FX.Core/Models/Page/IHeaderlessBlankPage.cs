using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Models.Page
{
    public interface IHeaderlessBlankPage : Base.IPage
    {
        string Head { get; set; }
        string Footer { get; set; }
        string BodyClass { get; set; }
    }
}
