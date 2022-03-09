using FX.Core.Models.Base;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Components.SocialMedia
{
    public interface ISocialFeed  : ISitecoreItem
    {
        string Title { get; set; }
        bool NarrowMode { get; set; }

        string FBText { get; set; }
        Image FBIcon { get; set; }
        Link FBLink { get; set; }
        bool FBShow { get; set; }

        string TWText { get; set; }
        Image TWIcon { get; set; }
        Link TWLink { get; set; }
        bool TWShow { get; set; }
        string TWScreenName { get; set; }
        string TweetID { get; set; }

        string LIText { get; set; }
        Image LIIcon { get; set; }
        Link LILink { get; set; }
        bool LIShow { get; set; }

        string YTText { get; set; }
        Image YTIcon { get; set; }
        Link YTLink { get; set; }
        string YTVideoID { get; set; }
        bool YTShow { get; set; }
    }
}
