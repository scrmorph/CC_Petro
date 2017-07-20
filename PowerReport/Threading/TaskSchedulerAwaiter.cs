using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace PowerReport.Threading
{
    public struct TaskSchedulerAwaiter : INotifyCompletion
    {
        private readonly TaskScheduler _taskScheduler;

        public TaskSchedulerAwaiter(TaskScheduler scheduler)
        {
            _taskScheduler = scheduler;
        }

        public bool IsCompleted
        {
            // Note: Check for the current scheduler is not sufficient because Scheduler is not associated with a thread and it's possible for example for the UI thread to have TaskScheduler.Current equal to TaskScheduler.Default 
            // In fact the implementation of TaskScheduler.Current returns TaskScheduler.Default if there is no CurrentTask
            get
            {
                // The conditions are based on implementation in https://www.nuget.org/packages/Microsoft.VisualStudio.Threading/

                // We can reuse the current scheduler/thread without scheduling a new task if the requested scheduler is TaskScheduler.Default and the current thread is a thread pool thread
                if (_taskScheduler == TaskScheduler.Default & Thread.CurrentThread.IsThreadPoolThread)
                    return true;

                // If the requested scheduler is the same as the current scheduler we only can reuse it if it's not default
                if (_taskScheduler == TaskScheduler.Current)
                    return TaskScheduler.Current != TaskScheduler.Default;

                // In all other cases we have to schedule a new task
                return false;
            }
        }

        public void OnCompleted(Action continuation)
        {
            Task.Factory.StartNew(continuation, CancellationToken.None, TaskCreationOptions.None, _taskScheduler);
        }

        public void GetResult()
        {
        }
    }
}
