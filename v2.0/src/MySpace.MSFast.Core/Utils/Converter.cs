using System;
using System.Collections.Generic;
using System.Text;

namespace MySpace.MSFast.Core.Utils
{
    public static class Converter
    {
        public static DateTime JAN_01_1970 = DateTime.SpecifyKind(new DateTime(1970, 1, 1, 0, 0, 0), DateTimeKind.Utc);

        public static DateTime EpochToDateTime(long time)
        {
            return JAN_01_1970.ToLocalTime().AddMilliseconds(time);
        }

        public static long DateTimeToEpoch(DateTime date)
        {
            DateTime dt = date.ToUniversalTime();
            TimeSpan ts = dt.Subtract(JAN_01_1970);
            return (long)ts.TotalMilliseconds;
        }
    }
}
