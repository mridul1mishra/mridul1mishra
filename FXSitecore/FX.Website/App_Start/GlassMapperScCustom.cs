using FX.Core.GlassMapper.DataHandler;
using FX.Core.GlassMapper.LangaugeItemMapConfiguration;
using FX.Core.GlassMapper.ProductCategoryMapConfiguration;
using Glass.Mapper.Configuration;
using Glass.Mapper.IoC;
using Glass.Mapper.Maps;
using Glass.Mapper.Sc.IoC;
using IDependencyResolver = Glass.Mapper.Sc.IoC.IDependencyResolver;

namespace FX.Website.App_Start
{
    public static  class GlassMapperScCustom
    {
		public static IDependencyResolver CreateResolver(){
			var config = new Glass.Mapper.Sc.Config();

            // This line was added to support SCContext.GetItem<T> with site-wide language fallback. Otherwise it will only work with item-level.
            config.DisableVersionCount = true; 

			var dependencyResolver = new DependencyResolver(config);
			// add any changes to the standard resolver here
			dependencyResolver.DataMapperFactory.Insert(0, () => new MultiLineTextDataMapper());
			dependencyResolver.DataMapperFactory.Insert(0, () => new EncodedStringValueDataMapper());
			dependencyResolver.DataMapperFactory.Insert(0, () => new TimeSlotValueDataMapper());
            dependencyResolver.DataMapperFactory.Insert(0, () => new LocalisedDateTimeDataMapper());
			return dependencyResolver;
		}

		public static IConfigurationLoader[] GlassLoaders(){
			
			/* USE THIS AREA TO ADD FLUENT CONFIGURATION LOADERS
             * 
             * If you are using Attribute Configuration or automapping/on-demand mapping you don't need to do anything!
             * 
             */
			var attributes = new Glass.Mapper.Configuration.Attributes.AttributeConfigurationLoader("FX.Core");
			return new IConfigurationLoader[] { attributes };
			//return new IConfigurationLoader[] { };
		}
		public static void PostLoad(){
			//Remove the comments to activate CodeFist
			/* CODE FIRST START
            var dbs = Sitecore.Configuration.Factory.GetDatabases();
            foreach (var db in dbs)
            {
                var provider = db.GetDataProviders().FirstOrDefault(x => x is GlassDataProvider) as GlassDataProvider;
                if (provider != null)
                {
                    using (new SecurityDisabler())
                    {
                        provider.Initialise(db);
                    }
                }
            }
             * CODE FIRST END
             */
		}
		public static void AddMaps(IConfigFactory<IGlassMap> mapsConfigFactory)
        {
            mapsConfigFactory.Add(() => new LanguageItemMap());
            mapsConfigFactory.Add(() => new ProductCategoryMap());
           
			// Add maps here
            // mapsConfigFactory.Add(() => new SeoMap());
        }
    }
}
