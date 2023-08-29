
using BN_Project.Core.Jobs;
using BN_Project.Core.Services.Implementations;
using BN_Project.Core.Services.Interfaces;
using BN_Project.Data.Repository;
using BN_Project.Domain.IRepository;
using EP.Core.Tools.RenderViewToString;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz.Impl;
using Quartz.Spi;
using Quartz;

namespace BN_Project.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IColorRepository, ColorRepository>();
            services.AddScoped<IGalleryRepository, GalleryRepository>();
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddScoped<IDiscountProductRepository, DiscountProductRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<ISectionRepository, SectionRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<ITicketMessageRepository, TicketMessageRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IImpressionsRepository, ImpressionRepository>();
            services.AddScoped<ICommentStrengthRepository, CommentStrengthRepository>();
            services.AddScoped<ICommentWeakPointsRepository, CommentWeakPointsRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();

            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IProductServices, ProductServices>();
            services.AddScoped<IViewRenderService, RenderViewToString>();
            services.AddScoped<ITicketServices, TicketServices>();
            services.AddScoped<ICommentServices, CommentServices>();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IOrderServices, OrderServices>();

            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            services.AddSingleton<CheckOrderDetails>();
            services.AddSingleton(new JobSchedule(jobType: typeof(CheckOrderDetails), cronExpression: "* 0/5 * * * ?"));
        }
    }
}
