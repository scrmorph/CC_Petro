using System.Threading.Tasks;

namespace PowerReport.Threading
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ContextScheduler 
    {
        public ContextScheduler(string name, TaskScheduler taskScheduler)
        {
            Name = name;
            this.TaskScheduler = taskScheduler;
        }

        public static implicit operator TaskScheduler(ContextScheduler contextScheduler)
        {
            return contextScheduler.TaskScheduler;
        }

        public string Name { get; private set; }
        public TaskScheduler TaskScheduler { get; protected set; }

        public abstract bool IsAsyncScheduler { get; }

        public TaskSchedulerAwaiter GetAwaiter()
        {
            return new TaskSchedulerAwaiter(this);
        }
    }
}
