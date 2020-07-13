using System;

namespace Omipay.CoreHelper
{
    public class DateHelper
    {
        /// <summary>
        /// 转换成北京时间
        /// </summary>
        /// <param name="dateTime">源时间</param>
        /// <param name="originTimezone">源时间的时区</param>
        /// <returns></returns>
        public static DateTime ConvertBeijingDateTime(DateTime dateTime, TimeZoneInfo originTimezone)
        {
            var utcTime = ConvertToUTC(dateTime, originTimezone);
            var estTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, TimeZoneInfoEx.BeijingTimeTimeZoneInfo);
            return estTime;
        }

        /// <summary> 
        /// 转换成澳洲东部时间
        /// </summary>
        /// <param name="dateTime">源时间</param>
        /// <param name="originTimezone">源时间的时区</param>
        /// <returns></returns>
        public static DateTime ConvertAustraliaEastDateTime(DateTime dateTime, TimeZoneInfo originTimezone) 
        {
            var utcTime = ConvertToUTC(dateTime, originTimezone);
            var estTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, TimeZoneInfoEx.AustraliaEastTimeZoneInfo);
            return estTime;
        }

        public static DateTime ConvertToUTC( DateTime dateTime, TimeZoneInfo originTimezone)
        {
            DateTime utcTime;
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                utcTime = TimeZoneInfo.ConvertTimeToUtc(dateTime, originTimezone);
            }
            else if (dateTime.Kind == DateTimeKind.Local)
            {
                utcTime = TimeZoneInfo.ConvertTimeToUtc(dateTime);
            }
            else
            {
                utcTime = dateTime;
            }
            return utcTime;
        }

        /// <summary>
        /// 获取utc时间戳（毫秒）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static Int64 GetUnixTimestamp(DateTime dateTime) 
        {
            TimeSpan ts = dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }

        /// <summary>
        /// 获取当前时间戳（秒）
        /// </summary>
        public static long GetTimeStamp() 
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
    }

    public static class TimeZoneInfoEx 
    {
        public static TimeZoneInfo BeijingTimeTimeZoneInfo => TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");

        public static TimeZoneInfo AustraliaEastTimeZoneInfo => TimeZoneInfo.FindSystemTimeZoneById(@"AUS Eastern Standard Time");
    }

}
