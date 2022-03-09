using FX.Core.Models.Base;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.GlassMapper.LangaugeItemMapConfiguration
{
    public class LanguageItemMap : SitecoreGlassMap<ILanguage>
    {
        public override void Configure()
        {
            Map(m => m.AutoMap(),
            m => m.Delegate(l => l.AssociatedLanguage).GetValue(
                    args => {
                        var language = Sitecore.Globalization.Language.Parse(!string.IsNullOrEmpty(args.Item["Regional Iso Code"]) ?
                            args.Item["Regional Iso Code"] :
                            args.Item["Iso"]);
                        return language;
                        }
                    ),
            m => m.Delegate(l => l.IconUrl).GetValue(
                    args => {
                            var language = Sitecore.Globalization.Language.Parse(!string.IsNullOrEmpty(args.Item["Regional Iso Code"]) ?
                                args.Item["Regional Iso Code"] : 
                                args.Item["Iso"]);

                            return "/~/icon/" + language.GetIcon(args.Item.Database);
                            }
                    ));
        }
    }
}
