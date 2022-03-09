using FX.Core.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Models.Components.Notifications
{
    public interface INotification : ISitecoreItem
    {
        string Text { get; set; }
    }
}
