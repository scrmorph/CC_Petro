using System;
using System.Collections.Generic;
using System.Linq;
using Services;

namespace PowerReport.Exporter
{
    public class ForwardReport
    {     
        private double[] _accumulatedVolumes;        

        public ForwardReport(): this(DateTime.Now)
        {
        }

        public ForwardReport(DateTime reportReportDate)
        {
            ReportDate = reportReportDate;
            _accumulatedVolumes = new double[24];
        }

        public DateTime ReportDate
        {
            get;
            private set;
        }

        public DateTime ReportRunDateTime
        {
            get;
            private set;
        }

        public ForwardReport WithPowerTrades(IEnumerable<PowerTrade> powerTrades)
        {
            _accumulatedVolumes =  powerTrades.SelectMany(trades => trades.Periods)
                                              .GroupBy(periods => periods.Period)
                                              .Select(group => new { Id = group.Key, Volume = group.Sum(p => p.Volume)})
                                              .OrderBy(data => data.Id).Select(data => data.Volume).ToArray();
            return this;
        }

        public void AddPowerTrade(PowerTrade powerTrade)
        {
            if (powerTrade.Date != ReportDate.Date)
            {
                throw new ArgumentException("PowerTrade has different date than report. Expected date is: " + ReportDate.Date + " trades date is: " + powerTrade.Date);
            }

            for (int i = 0; i < powerTrade.Periods.Length; i++)
            {
                var period = powerTrade.Periods[i];
                _accumulatedVolumes[period.Period] += period.Volume;
            }
        }

        public ForwardReportPosition[] GetPositions()
        {
            var positions = new ForwardReportPosition[_accumulatedVolumes.Length];
            for (int i = 0; i < _accumulatedVolumes.Length; i++)
            {
                var time = i == 0 ? TimeSpan.FromHours(23) : TimeSpan.FromHours(i - 1);
                positions[i] = new ForwardReportPosition(i, time, _accumulatedVolumes[i]);
            }
            return positions;
        }

        
    }
}
