using BN_Project.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BN_Project.Core.Jobs
{
    public class SingletonJobFactory : IJobFactory
    {

        private readonly IServiceProvider _serviceProvider;

        public SingletonJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            Console.WriteLine("fff");
            var jobDetail = bundle.JobDetail;

            var result = _serviceProvider.GetRequiredService(jobDetail.JobType) as IJob;

            return result;
        }

        public void ReturnJob(IJob job)
        {
            
        }
    }
}
