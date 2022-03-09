using FX.Core.Models.Settings;
using Sitecore.ContentSearch.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.GlassMapper.DataHandler
{
    public class LocalisedDateTimeValue
    {
        public string RawValue { get; set; }
        public DateTime DateTime;
        private ISiteSettings SiteSettings;
        private CultureInfo CultureInfo;
        public LocalisedDateTimeValue(string s)
        {
            SiteSettings = FXContextItems.HomePage.SiteSettings;
            try
            {
                if(SiteSettings.Language=="en")
                {
                    CultureInfo = CultureInfo.CurrentCulture;
                }
                else
                {
                    CultureInfo = CultureInfo.CreateSpecificCulture(SiteSettings.Language);
                }
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(SiteSettings.TimeZone);
                DateTime UTCDate = Sitecore.DateUtil.ParseDateTime(s, DateTime).ToUniversalTime();
                DateTime newDate = TimeZoneInfo.ConvertTimeFromUtc(UTCDate, tzi);

                DateTime = newDate;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Cannot parse localised Date Time" + "\n" + ex.Message + "\n" + ex.StackTrace, this);
            }
        }

        public LocalisedDateTimeValue(DateTime s)
        {
            SiteSettings = FXContextItems.HomePage.SiteSettings;
            try
            {
                if (SiteSettings.Language == "en")
                {
                    CultureInfo = CultureInfo.CurrentCulture;
                }
                else
                {
                    CultureInfo = CultureInfo.CreateSpecificCulture(SiteSettings.Language);
                }
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(SiteSettings.TimeZone);
                DateTime newDate = TimeZoneInfo.ConvertTimeFromUtc(s, tzi);

                DateTime = newDate;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Cannot parse localised Date Time" + "\n" + ex.Message + "\n" + ex.StackTrace, this);
            }
            
        }

        public LocalisedDateTimeValue(IndexFieldStorageValueFormatter indexFieldStorageValueFormatter) { }

        public override string ToString()
        {
            return this.DateTime.ToString("d", CultureInfo);
        }

        public string ToString(string s)
        {
            return this.DateTime.ToString(s, CultureInfo);
        }

        public string DayMonthYearString
        {
            get
            {
                return DateTime.ToString(SiteSettings.DayMonthYearFormat,CultureInfo);
            }
        }

        public string TimeString
        {
            get
            {
                return DateTime.ToString(SiteSettings.TimeFormat, CultureInfo);
            }
        }

        public string DateRange(DateTime startDate, DateTime endDate)
        {
            string output = "";

            if (startDate.Date == endDate.Date)
                return startDate.ToString(SiteSettings.DayMonthYearFormat, CultureInfo);

            if (startDate != DateTime.MinValue)
            {
                output = startDate.ToString(SiteSettings.DayMonthYearFormat, CultureInfo);
                if (endDate != DateTime.MinValue)
                {

                    if (startDate.Month == endDate.Month)
                    {
                        output = startDate.ToString(SiteSettings.DateRangeStartFormat, CultureInfo) + " - " + endDate.ToString(SiteSettings.DateRangeEndFormat, CultureInfo);
                    }
                    else if (startDate.Year != endDate.Year)
                    {
                        output = startDate.ToString(SiteSettings.DayMonthYearFormat, CultureInfo) + " - " + endDate.ToString(SiteSettings.DateRangeEndFormat, CultureInfo);
                    }
                    else
                    {
                        output = startDate.ToString(SiteSettings.DateRangeStartFormat, CultureInfo) + " - " + endDate.ToString(SiteSettings.DateRangeEndFormat, CultureInfo);
                    }

                }
            }

            return output;
        }
    }
}
