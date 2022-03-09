using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Models.Components.Notifications
{
    public interface INotificationFolder : ISitecoreItem
    {
        [SitecoreChildren(InferType = true)]
        IEnumerable<INotification> Children { get; set; }
    }
}
