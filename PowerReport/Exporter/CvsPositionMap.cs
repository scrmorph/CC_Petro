using CsvHelper.Configuration;

namespace PowerReport.Exporter
{
    /// <summary>
    /// Map used for Csv export. 
    /// </summary>
    public class CvsPositionMap : CsvClassMap<InterdayPowerReportPosition>
    {
        public CvsPositionMap()
        {
            Map(m => m.Hour).TypeConverterOption(@"hh\:mm").Name("Local Time");
            Map(m => m.Volume).Name("Volume");
        }
    }
}
