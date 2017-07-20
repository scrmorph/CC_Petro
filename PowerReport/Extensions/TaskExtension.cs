using System;
using System.Threading.Tasks;
using PowerReport.Threading;

namespace PowerReport.Extensions
{
    public static class TaskExtension
    {

        /// <summary>
        /// Delays a task using the ContextScheduler
        /// </summary>
        /// <param name="scheduler"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static async Task Delay(this ContextScheduler scheduler, TimeSpan delay)
        {
            if (scheduler.IsAsyncScheduler)
                await Task.Delay(delay);

            await scheduler;
        }

    }
}
