using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Models.DTO
{
    public class CareerFieldArgument<T>
    {
        public string Key { get; set; }
        public IEnumerable<T> Values { get; set; }
    }
    public class CareerSearch
    {
        public IEnumerable<CareerFieldArgument<string>> Arguments { get; set; }
        public CareerFieldArgument<string> DateItemArgument { get; set; }
    }
}
