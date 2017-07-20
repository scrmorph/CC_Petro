using System.Threading.Tasks;

namespace PowerReport.Threading
{
   
    public class BackgroundScheduler : ContextScheduler
    {
        static BackgroundScheduler()
        {            
            Default = new BackgroundScheduler("AsyncBackgroundScheduler", TaskScheduler.Default);
        }

        protected internal BackgroundScheduler(string name, TaskScheduler taskScheduler)
            : base(name, taskScheduler)
        {
        }

        public static BackgroundScheduler Default
        {
            get;         
            set;
        }

        public override bool IsAsyncScheduler { get; }
    }
}
