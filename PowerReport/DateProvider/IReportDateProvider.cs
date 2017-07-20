using System;

namespace PowerReport.DateProvider
{
    public interface IReportDateProvider
    {
        /// <summary>
        /// Returns date for getting data from service.        
        /// </summary>        
        DateTime GetReportDate();

        /// <summary>
        /// Returns date for report file name        
        /// </summary>        
        DateTime GetExtractDateTime();
    }
}
