using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestServiceConsoleApplication
{
    class JobsBuilder
    {
        private IScheduler scheduler;
        internal void CreateDataJobs()
        {
            CreateUploadDataJob();
        }
        private IJobDetail CreateUploadDataJob()
        {
            IJobDetail job = JobBuilder.Create<UploadDataJob>().WithIdentity("UploadDataJob", "DataServiceJobGroup").Build();
            //job.JobDataMap.Put("Spi", Spi);
            //使用组别、名称创建一个触发器，其中触发器立即执行，且每隔1秒执行一个任务，重复执行
            //ITrigger trigger = TriggerBuilder
            //    .Create()
            //    .WithIdentity("UploadDataTrigger", "UploadDataTriggerGroup")
            //    .StartNow()
            //    .WithSimpleSchedule(x => x.WithIntervalInSeconds(2).RepeatForever()).Build();
            ICronTrigger trigger = new CronTriggerImpl("UploadDataCronTrigger");
            trigger.CronExpressionString = "0/5 * * * * ?";
            //ITrigger trigger = TriggerBuilder.Create().StartNow().Build();
            StareddSchduler.ScheduleJob(job, trigger);
            return job;
        }
        IScheduler StareddSchduler
        {
            get
            {
                ISchedulerFactory factory = new StdSchedulerFactory();
                scheduler = scheduler ?? factory.GetScheduler();
                if (!scheduler.IsStarted)
                {
                    scheduler.Start();
                }
                return scheduler;
            }
        }
    }
}
