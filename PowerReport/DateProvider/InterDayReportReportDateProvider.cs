using System;

namespace PowerReport.DateProvider
{
    /// <summary>
    /// Class returns dates for InterDay Report.     
    /// </summary>
    public class InterDayReportReportDateProvider : IReportDateProvider
    {
        /// <summary>
        /// Report day starts at 23:00 
        /// </summary>        
        public DateTime GetReportDate()
        {
            //Gets local time, if we need to use UTC time we can use 
            //DateTime.UtcNow;
            var localTime = DateTime.Now;
            if (localTime.Hour == 23)
            {
                return localTime.AddHours(1).Date;
            }
            return localTime.Date;
        }

        /// <summary>
        /// Extract date (used for creating report filename)
        /// </summary>        
        public DateTime GetExtractDateTime()
        {
            //Gets local time, if we need to use UTC time we can use 
            //DateTime.UtcNow;
            return DateTime.Now;
        }

        /// <summary>
        /// Additional implementation, in case that date used in file name
        /// should be calculated basing on raport start date (23:00)        
        /// </summary>        
        //public DateTime GetExtractDateTime()
        //{
        //    var localTime = DateTime.Now;
        //    if (localTime.Hour == 23)
        //    {
        //        return localTime.AddHours(1).Date.Add(TimeSpan.FromMinutes(localTime.Minute));
        //    }
        //    return localTime.AddHours(1);
        //}
    }
}
