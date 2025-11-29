using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Shared.Helpers
{
    public static class TimeHelper
    {
        private static readonly TimeZoneInfo PhTimeZone = GetPhTimeZone();

        private static TimeZoneInfo GetPhTimeZone()
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
            }
            catch
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila");
            }
        }

      
        public static DateTime Now()
        {
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, PhTimeZone);
        }

       
        public static DateTime NextRunAt(int hour24)
        {
            var now = Now();
            var nextRun = now.Date.AddHours(hour24);

            if (now > nextRun)
                nextRun = nextRun.AddDays(1);

            return nextRun;
        }

   
        public static TimeSpan TimeUntilNextRun(int hour24)
        {
            return NextRunAt(hour24) - Now();
        }
        public static DateTime UtcToPh(DateTime utcDate)
        {
            if (utcDate.Kind == DateTimeKind.Unspecified)
                utcDate = DateTime.SpecifyKind(utcDate, DateTimeKind.Utc);

            return TimeZoneInfo.ConvertTime(utcDate, PhTimeZone);
        }

    }
}
