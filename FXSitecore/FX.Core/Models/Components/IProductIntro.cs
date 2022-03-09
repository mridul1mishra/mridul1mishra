using FX.Core.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Models.Components
{
    public interface IProductIntro : IRichText
    {
        IProductPage ProductPage { get; set; } 
    }
}
