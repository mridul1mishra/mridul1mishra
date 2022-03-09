using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.MyanmarCultureInfo.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                CultureAndRegionInfoBuilder.Unregister("my-MM");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Existing culture not found, proceeding to add.");
            }
            CultureAndRegionInfoBuilder builder =
              new CultureAndRegionInfoBuilder("my-MM",
              CultureAndRegionModifiers.None);

            builder.Parent = CultureInfo.InvariantCulture;
            builder.CultureEnglishName = "Burmese (Myanmar";
            builder.CultureNativeName = "ျမန္​မာစာ";
            builder.ThreeLetterISOLanguageName = "bur";
            builder.ThreeLetterWindowsLanguageName = "bur";
            builder.TwoLetterISOLanguageName = "my";

            builder.RegionEnglishName = "Myanmar";
            builder.RegionNativeName = "ျမန္​မာႏိုင္​ငံ";
            builder.ThreeLetterISORegionName = "mmr";
            builder.ThreeLetterWindowsRegionName = "mmr";
            builder.TwoLetterISORegionName = "mm";

            builder.IetfLanguageTag = "my-MM";

            builder.IsMetric = false;
            //builder.KeyboardLayoutId = 
            builder.GeoId = 0x1b;


            builder.GregorianDateTimeFormat = CreateBurmeseTimeFormatInfo();
            builder.NumberFormat = CreateBurmeseNumberFormatInfo();

            builder.CurrencyEnglishName = "Kyat";
            builder.CurrencyNativeName = "ကျပ်";
            builder.ISOCurrencySymbol = "K";

            builder.TextInfo = CultureInfo.InvariantCulture.TextInfo;

            builder.CompareInfo = CultureInfo.InvariantCulture.CompareInfo;

            builder.Register();

        }
        private static NumberFormatInfo CreateBurmeseNumberFormatInfo()
        {
            NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
            numberFormatInfo.CurrencyDecimalDigits = 2;
            numberFormatInfo.CurrencyDecimalSeparator = ".";
            numberFormatInfo.CurrencyGroupSeparator = ",";
            numberFormatInfo.CurrencyGroupSizes = new int[] { 3, 2 };
            numberFormatInfo.CurrencyNegativePattern = 12;
            numberFormatInfo.CurrencyPositivePattern = 2;
            numberFormatInfo.CurrencySymbol = "K";
            numberFormatInfo.DigitSubstitution = DigitShapes.None;
            numberFormatInfo.NaNSymbol = "NaN";
            numberFormatInfo.NativeDigits = new string[]
              { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            numberFormatInfo.NegativeInfinitySymbol = "-Infinity";
            numberFormatInfo.NegativeSign = "-";
            numberFormatInfo.NumberDecimalDigits = 2;
            numberFormatInfo.NumberDecimalSeparator = ".";
            numberFormatInfo.NumberGroupSeparator = ",";
            numberFormatInfo.NumberGroupSizes = new int[] { 3, 2 };
            numberFormatInfo.NumberNegativePattern = 1;
            numberFormatInfo.PercentDecimalDigits = 2;
            numberFormatInfo.PercentDecimalSeparator = ".";
            numberFormatInfo.PercentGroupSeparator = ",";
            numberFormatInfo.PercentGroupSizes = new int[] { 3, 2 };
            numberFormatInfo.PercentNegativePattern = 0;
            numberFormatInfo.PercentPositivePattern = 0;
            numberFormatInfo.PercentSymbol = "%";
            numberFormatInfo.PerMilleSymbol = "‰";
            numberFormatInfo.PositiveInfinitySymbol = "Infinity";
            numberFormatInfo.PositiveSign = "+";
            return numberFormatInfo;
        }
        private static DateTimeFormatInfo CreateBurmeseTimeFormatInfo()
        {
            Calendar calendar =
              new GregorianCalendar(GregorianCalendarTypes.Localized);

            DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();

            dateTimeFormatInfo.Calendar = calendar;

            dateTimeFormatInfo.AbbreviatedDayNames = new string[]
              { "တနလၤာ", "အဂၤါ", "ဗုဒၶဟူး", "ၾကာသပေတး", "ေသာၾကာ", "စေန", "တနဂၤ​ေႏြ" };
            dateTimeFormatInfo.DayNames = new string[] { "တနလၤာ", "အဂၤါ", "ဗုဒၶဟူး", "ၾကာသပေတး", "ေသာၾကာ", "စေန", "တနဂၤ​ေႏြ" };
            dateTimeFormatInfo.ShortestDayNames = new string[]
             { "တနလၤာ", "အဂၤါ", "ဗုဒၶဟူး", "ၾကာသပေတး", "ေသာၾကာ", "စေန", "တနဂၤ​ေႏြ" };

            dateTimeFormatInfo.AbbreviatedMonthNames = new string[]
              { "ဇန္နဝါရီ", "ေဖေဖာ္ဝါရီ", "မတ္", "ဧပရယ္", "ေမ", "ဇြန္", "ဇြန္လိုင္", "ၾသဂုတ္", "စက္​တင္​ဘာ", "ေအာက္တိုဘာ", "ႏိုဝင္ဘာ", "ဒီဇင္ဘာ", "" };
            dateTimeFormatInfo.MonthNames = new string[]
               { "ဇန္နဝါရီ", "ေဖေဖာ္ဝါရီ", "မတ္", "ဧပရယ္", "ေမ", "ဇြန္", "ဇြန္လိုင္", "ၾသဂုတ္", "စက္​တင္​ဘာ", "ေအာက္တိုဘာ", "ႏိုဝင္ဘာ", "ဒီဇင္ဘာ", "" };

            dateTimeFormatInfo.AbbreviatedMonthGenitiveNames =
              new string[] { "ဇန္နဝါရီ", "ေဖေဖာ္ဝါရီ", "မတ္", "ဧပရယ္", "ေမ", "ဇြန္", "ဇြန္လိုင္", "ၾသဂုတ္", "စက္​တင္​ဘာ", "ေအာက္တိုဘာ", "ႏိုဝင္ဘာ", "ဒီဇင္ဘာ", "" };
            dateTimeFormatInfo.MonthGenitiveNames = new string[] { "ဇန္နဝါရီ", "ေဖေဖာ္ဝါရီ", "မတ္", "ဧပရယ္", "ေမ", "ဇြန္", "ဇြန္လိုင္", "ၾသဂုတ္", "စက္​တင္​ဘာ", "ေအာက္တိုဘာ", "ႏိုဝင္ဘာ", "ဒီဇင္ဘာ", "" };

            dateTimeFormatInfo.AMDesignator = "";
            dateTimeFormatInfo.CalendarWeekRule = CalendarWeekRule.FirstDay;
            dateTimeFormatInfo.DateSeparator = "-";
            dateTimeFormatInfo.FirstDayOfWeek = DayOfWeek.Monday;
            dateTimeFormatInfo.FullDateTimePattern = "dd MMMM yyyy HH:mm:ss";
            dateTimeFormatInfo.LongDatePattern = "dd MMMM yyyy";
            dateTimeFormatInfo.LongTimePattern = "HH:mm:ss";
            dateTimeFormatInfo.MonthDayPattern = "dd MMMM";
            dateTimeFormatInfo.PMDesignator = "";
            dateTimeFormatInfo.ShortDatePattern = "dd-MM-yyyy";
            dateTimeFormatInfo.ShortTimePattern = "HH:mm";
            dateTimeFormatInfo.TimeSeparator = ":";
            dateTimeFormatInfo.YearMonthPattern = "MMMM, yyyy";

            return dateTimeFormatInfo;
        }
    }
}
