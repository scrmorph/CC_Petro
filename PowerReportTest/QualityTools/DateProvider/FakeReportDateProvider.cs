using System;
using PowerReport.DateProvider;

namespace PowerReportTest.QualityTools.DateProvider
{
    public class FakeReportDateProvider : IReportDateProvider
    {
        private DateTime _dateTime;
        public FakeReportDateProvider WithCurrentDateAndTime(DateTime current)
        {
            _dateTime = current;
            return this;
        }

        public DateTime GetReportDate()
        {
            var localTime = _dateTime;
            if (localTime.Hour == 23)
            {
                return localTime.AddHours(1).Date;
            }
            return localTime.Date;
        }

        public DateTime GetExtractDateTime()
        {
            return _dateTime;
        }
    }
}
