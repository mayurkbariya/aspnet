using System;
using TimeZoneConverter;

namespace FBDropshipper.Common.Extensions
{
    public static class DateExtension
    {
        public static DateTime ToDateTimeZoneUtc(this DateTime dateTime, string timeZone)
        {
            if (timeZone.IsNullOrWhiteSpace())
            {
                return dateTime;
            }
            var tzi = TZConvert.GetTimeZoneInfo(timeZone);
            return TimeZoneInfo.ConvertTimeToUtc(dateTime.Date, tzi);
        }
        public static DateTime ToDateTimeZoneUtc(this DateTime? dateTime, string timeZone)
        {
            if (timeZone.IsNullOrWhiteSpace() || dateTime == null)
            {
                return dateTime ?? DateTime.MinValue;
            }
            var tzi = TZConvert.GetTimeZoneInfo(timeZone);
            return TimeZoneInfo.ConvertTimeToUtc(dateTime.Value.Date, tzi);
        }
        public static string ToGeneralDateTime(this long ticks)
        {
            if (ticks == 0)
            {
                return "";
            }
            DateTime myDate = new DateTime(ticks);
            return myDate.ToGeneralDateTime();
        }
        public static string ToDayAndDate(this DateTime dateTime)
        {
            return dateTime.ToString("ddd dd-MMM");
        }
        public static string ToDayAndDate(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.GetValueOrDefault().ToString("ddd dd-MMM");
            }
            return "";
        }
        public static string ToShortDate(this DateTime dateTime)
        {
            return dateTime.ToShortDateString();
        }
        public static string ToGeneralDateTime(this DateTime dateTime)
        {
            return dateTime.ToString("u");
        }
        public static string ToGeneralDate(this DateOnly dateTime)
        {
            return dateTime.ToString();
        }
        public static string ToGeneralDateTime(this DateTime? dateTime)
        {
            return !dateTime.HasValue ? "" : dateTime.GetValueOrDefault().ToString("u");
        }

        public static string ToUtc(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToGeneralDateTime();
        }
        
    }
}