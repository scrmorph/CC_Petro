using PowerReport.Threading;

namespace PowerReportTest.QualityTools.Threading
{
    public class InstantBackgroundScheduler : BackgroundScheduler
    {
        public InstantBackgroundScheduler() : base("InstantBackgroundScheduler", InstantTaskScheduler.Default)
        {
        }

        public override bool IsAsyncScheduler { get { return false; } }
    }
}
