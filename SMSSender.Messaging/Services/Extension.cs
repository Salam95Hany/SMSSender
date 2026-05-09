using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging.Services
{
    public static class Extension
    {
        public static DateTime EgyptNow(this DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime.ToUniversalTime(), "Egypt Standard Time");
        }
    }
}
