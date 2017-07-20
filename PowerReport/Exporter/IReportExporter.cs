using System;

namespace PowerReport.Exporter
{
    /// <summary>
    /// Defines interface of report exporter.
    /// </summary>
    public interface IReportExporter
    {
        /// <summary>
        /// Exports report to destination.
        /// </summary>
        /// <param name="destination">Destination (ex. Output path)</param>
        /// <param name="reportDateTime">Date Time of report</param>
        /// <param name="positions">InterDay positions</param>
        void Export(string destination, DateTime reportDateTime, InterdayPowerReportPosition[] positions);
    }
}
