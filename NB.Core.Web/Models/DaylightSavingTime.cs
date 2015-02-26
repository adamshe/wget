using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NB.Core.Web.Models
{
    public class DaylightSavingTime
    {

        private int mYear;
        private DateTime mStartDate;

        private DateTime mEndDate;
        public int Year
        {
            get { return mYear; }
        }
        public DateTime StartDate
        {
            get { return mStartDate; }
        }
        public DateTime EndDate
        {
            get { return mEndDate; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="startDate">The start date of Daylight Saving Time</param>
        /// <param name="endDate">The end date of Daylight Saving Time</param>
        /// <remarks>In case of countries in southern hemisphere the start date is higher then the end date. In this case the object contains the end date of the period that starts one year before. The start date is the begin of the period that will go on into the next year.</remarks>
        public DaylightSavingTime(DateTime startDate, DateTime endDate)
        {
            if (startDate.Year != endDate.Year)
            {
                throw new ArgumentException("The year of [startDate] is not the same year like [endDate] parameter.", "startDate");
            }
            else
            {
                mYear = startDate.Year;
                mStartDate = startDate;
                mEndDate = endDate;
            }
        }

    }
}
