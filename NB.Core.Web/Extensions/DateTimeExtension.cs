using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime ThisMonthNthOf(this DateTime curDate, int occurrence, DayOfWeek dayOfWeek)
        {
            var startDayOfCurrentMonth = new DateTime(curDate.Year, curDate.Month, 1);

            var monthStartOfTargetDayOfWeek = startDayOfCurrentMonth.DayOfWeek == dayOfWeek ? startDayOfCurrentMonth : startDayOfCurrentMonth.AddDays(dayOfWeek - startDayOfCurrentMonth.DayOfWeek);
            // CurDate = 2011.10.1 Occurance = 1, Day = Friday >> 2011.09.30 FIX. 
            if (monthStartOfTargetDayOfWeek.Month < curDate.Month) 
                occurrence = occurrence + 1;
            return monthStartOfTargetDayOfWeek.AddDays(7 * (occurrence - 1));
        }

        public static IEnumerable<DateTime> ThisYearNthOf(this DateTime curDate, int occurrence, DayOfWeek dayOfWeek)
        {
            var list = new List<DateTime>(12);
            var startDayOfCurrentMonth = new DateTime(curDate.Year, curDate.Month, 1);
            var nextYear = startDayOfCurrentMonth.AddYears(1).Year;
            var dateIterator = startDayOfCurrentMonth;
            while (dateIterator.Year < nextYear)
            {
                dateIterator = dateIterator.AddMonths(1);
                var date = dateIterator.ThisMonthNthOf(3, DayOfWeek.Friday);
                yield return date;

            }
        }
    }
}
