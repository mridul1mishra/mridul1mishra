using FX.Core.GlassMapper.DataHandler;
using FX.Core.Models.Base;
using FX.Core.Models.Page;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;

namespace FX.Core.Models.Components
{
	public interface ILatestEvents : ISitecoreItem
	{
		#region Content
		string ListingTitle { get; set; }
		string ListingLinkText { get; set; }
		Link ListingSource { get; set; }
		string EmptyListingText { get; set; }
		#endregion

        /* Event Links not used anymore due to CC003 enhancement */
		#region Event Link 1
		string EventTitle1 { get; set; }
		DateTime EventDate1 { get; set; }

        [SitecoreField("EventDate1")]
        LocalisedDateTimeValue LocalisedEventDate1 { get; set; }

        DateTime EventEndDate1 { get; set; }

        [SitecoreField("EventEndDate1")]
        LocalisedDateTimeValue LocalisedEventEndDate1 { get; set; }

        Link EventLink1 { get; set; }
		#endregion

		#region Event Link 2
		string EventTitle2 { get; set; }
		DateTime EventDate2 { get; set; }
        [SitecoreField("EventDate2")]
        LocalisedDateTimeValue LocalisedEventDate2 { get; set; }
        DateTime EventEndDate2 { get; set; }
        [SitecoreField("EventEndDate2")]
        LocalisedDateTimeValue LocalisedEventEndDate2 { get; set; }
        Link EventLink2 { get; set; }
		#endregion

		#region Event Link 3
		string EventTitle3 { get; set; }
		DateTime EventDate3 { get; set; }
        [SitecoreField("EventDate3")]
        LocalisedDateTimeValue LocalisedEventDate3 { get; set; }
        DateTime EventEndDate3 { get; set; }
        [SitecoreField("EventEndDate3")]
        LocalisedDateTimeValue LocalisedEventEndDate3 { get; set; }
        Link EventLink3 { get; set; }
		#endregion

		#region Event Link 4
		string EventTitle4 { get; set; }
		DateTime EventDate4 { get; set; }
        [SitecoreField("EventDate4")]
        LocalisedDateTimeValue LocalisedEventDate4 { get; set; }
        DateTime EventEndDate4 { get; set; }
        [SitecoreField("EventEndDate4")]
        LocalisedDateTimeValue LocalisedEventEndDate4 { get; set; }
        Link EventLink4 { get; set; }
		#endregion
        /* Event Links not used anymore due to CC003 enhancement */
	}
}
