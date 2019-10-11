using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiRest
{
    public class JobScheduler
    {
        public static void Start()
        {
            var schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = schedulerFactory.GetScheduler().Result;

            scheduler.Start();

            IJobDetail job = JobBuilder.Create<ForecastJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()

                .WithIdentity("ForecastJob", "Forecast")

                .WithCronSchedule("0 50 21 1/1 * ? *")

                .StartAt(DateTime.UtcNow)

                .WithPriority(1)

                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}