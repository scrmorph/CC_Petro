using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PowerReportTest.QualityTools.Threading
{
    public class InstantTaskScheduler : TaskScheduler
    {
        public new static readonly InstantTaskScheduler Default = new InstantTaskScheduler();

        public override int MaximumConcurrencyLevel
        {
            get { return 1; }
        }

        protected override void QueueTask(Task task)
        {
            TryExecuteTask(task);
        }

        [DebuggerStepThrough]
        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return TryExecuteTask(task);
        }

        [DebuggerStepThrough]
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return Enumerable.Empty<Task>();
        }

        
    }
}
