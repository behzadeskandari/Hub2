using System.Globalization;
using System.Runtime.InteropServices;

namespace Yarane.Common.Utilities
{
    public static class DateTimeExtensions
    {
        public static string ToDateTime(this DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            return $"{pc.GetYear(DateTime.Now):0000}{pc.GetMonth(DateTime.Now):00}{pc.GetDayOfMonth(DateTime.Now):00}_{pc.GetHour(DateTime.Now):00}{pc.GetMinute(DateTime.Now):00}{pc.GetSecond(DateTime.Now):00}";
        }

        //Install-Package PersianDateTime
        public static string ConvertToGregorianDate(this string solarDate)
        {
            // Parse the solar date string into year, month, and day parts
            string[] parts = solarDate.Split('/');
            int year = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int day = int.Parse(parts[2]);

            PersianCalendar persianCalendar = new PersianCalendar();

            DateTime gregorianDate = persianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);

            string gregorianDateString = gregorianDate.ToString("yyyy-MM-dd");

            return gregorianDateString;
        }

        public static string GetIranianDateTime(this DateTime dateTime)
        {
            var iranTime = dateTime.GetIranianTimeZone();
            PersianCalendar pc = new PersianCalendar();
            string year = pc.GetYear(iranTime).ToString();
            string month = pc.GetMonth(iranTime).ToString().PadLeft(2, '0');
            string day = pc.GetDayOfMonth(iranTime).ToString().PadLeft(2, '0');
            string hour = iranTime.Hour.ToString("00");
            string minute = iranTime.Minute.ToString("00");
            string second = iranTime.Second.ToString("00");
            string millisecond = iranTime.Millisecond.ToString().PadLeft(3, '0');

            return $"{year}/{month}/{day} - {hour}:{minute}:{second}.{millisecond}";
        }
        public static string GetIranianDateTimeWithDash(this DateTime dateTime)
        {
            var iranTime = dateTime.GetIranianTimeZone();
            PersianCalendar pc = new PersianCalendar();
            string year = pc.GetYear(iranTime).ToString();
            string month = pc.GetMonth(iranTime).ToString().PadLeft(2, '0');
            string day = pc.GetDayOfMonth(iranTime).ToString().PadLeft(2, '0');
            string hour = iranTime.Hour.ToString("00");
            string minute = iranTime.Minute.ToString("00");
            string second = iranTime.Second.ToString("00");
            string millisecond = iranTime.Millisecond.ToString().PadLeft(3, '0');

            return $"{year}/{month}/{day} {hour}:{minute}";
        }
        public static string GetIranianDateTimeWithOutDash(this DateTime dateTime)
        {
            var iranTime = dateTime.GetIranianTimeZone();
            PersianCalendar pc = new PersianCalendar();
            string year = pc.GetYear(iranTime).ToString();
            string month = pc.GetMonth(iranTime).ToString().PadLeft(2, '0');
            string day = pc.GetDayOfMonth(iranTime).ToString().PadLeft(2, '0');
            string hour = iranTime.Hour.ToString("00");
            string minute = iranTime.Minute.ToString("00");
            string second = iranTime.Second.ToString("00");
            string millisecond = iranTime.Millisecond.ToString().PadLeft(3, '0');

            return $"{year}{month}{day}";
        }

        public static string GetIranianDate(this DateTime dateTime)
        {
            var iranTime = dateTime.GetIranianTimeZone();
            PersianCalendar pc = new PersianCalendar();
            string year = pc.GetYear(iranTime).ToString();
            string month = pc.GetMonth(iranTime).ToString().PadLeft(2, '0');
            string day = pc.GetDayOfMonth(iranTime).ToString().PadLeft(2, '0');

            return $"{year}/{month}/{day}";
        }
        public static DateTime GetIranianTimeZone(this DateTime dateTime)
        {
            TimeZoneInfo timeZone = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time") : TimeZoneInfo.FindSystemTimeZoneById("Asia/Tehran");
            DateTime iranTime = TimeZoneInfo.ConvertTime(dateTime, timeZone);
            return iranTime;
        }
    }
}
