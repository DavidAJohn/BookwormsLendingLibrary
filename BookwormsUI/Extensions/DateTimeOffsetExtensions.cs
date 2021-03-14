using System;

namespace BookwormsUI.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        public static string ToUKStandardDate(this DateTimeOffset dateTime)
        {
            // returns date as dd/mm/yy (eg. 14/03/2021)
            return String.Format("{0:dd/MM/yyyy}", dateTime);
        }
    }
}
