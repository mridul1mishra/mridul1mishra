using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core
{
    public class Constants
    {
        public struct MaxLength
        {
            public const int Title = 50;
            public const int AccordionTitle = 30;
            public const int TakeNextStepSectionTitle = 20;
            public const int SectionTitle = 20;
            public const int FeaturedTitle = 15;

            public const int FeaturedDescription = 100;
            public const int TeaserDescription = 120;
            public const int BlogDescription = 130;
            public const int BannerDescription = 140;
        }

        public struct SnSTaxonomy
        {
            public const string TaxonomySeparator = "|";
            public const string TagSeparator = "/";
        }

        public struct RelatedComponent
        {
            public const int NumberOfItems = 3;
            public const int NumberOfItemsForProductsAndServices = 2;
        }

        public struct LatestNewsComponent
        {
            public const int NumberOfItems = 4;
        }

        public struct DateTimeFormat
        {
            public const string TimeOnly = "HHmm";
            public const string DateOnly = "MMM d, yyyy";
            public const string DateOnlyDateFirst = "dd MMM yyyy";
            public const string FullDateTime = "MMM dd yyyy HH:mm:ss tt";
        }
        public struct Footer
        {
            public const string FooterMainNavigationId = "{9B3CAEE6-8283-4DDA-9A7B-140E143260B7}";
            public const string NavigableTemplateId = "{08CC1A04-3D3F-4A66-A85F-064378AA9A8B}";
            public const string TitleId = "{39C597A5-B4F5-4B8D-9177-EFD773C40D56}";
            public const string LinkId = "{D34ABDA0-0FB2-46C8-8BEA-40083A397108}";
            public const string LevelId = "{52318C1D-E1F3-4F9B-9D3C-55E90F03D508}";
        }
        public struct CaptchaConstant
        {
            public const string recaptchaItemId = "{A69D1B5F-241A-4A2E-BE94-D48E78194372}";
            public const string captchaViewItem = "{D503A938-9954-42E2-B3BA-54E073DEA67E}";
        }
        public struct AnnouncementConstant
        {
            public const string AnnouncementItemCountFieldName = "AnnouncementItemCount";
        }

        public struct LinkTypeConstant
        {
            public const string Internal = "internal";
            public const string External = "external";
        }
    }

}
