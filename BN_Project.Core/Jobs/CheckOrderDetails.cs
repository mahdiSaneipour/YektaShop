using BN_Project.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace BN_Project.Core.Jobs
{
    [DisallowConcurrentExecution]
    public class CheckOrderDetails : IJob
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CheckOrderDetails(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine(DateTime.Now + " test");

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IOrderServices>();

                service.CheckOrderDetailsInBasket();
            }

            return Task.CompletedTask;
        }
    }
}
