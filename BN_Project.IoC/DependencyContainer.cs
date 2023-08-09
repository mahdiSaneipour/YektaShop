
using BN_Project.Core.Services.Implementations;
using BN_Project.Core.Services.Interfaces;
using BN_Project.Data.Repository;
using BN_Project.Domain.IRepository;
using EP.Core.Tools.RenderViewToString;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddScoped<ITicketMessageRepository, TicketMessageRepository>();

            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IProductServices, ProductServices>();
            services.AddScoped<IViewRenderService, RenderViewToString>();
            services.AddScoped<IProfileService, ProfileServices>();
        }
    }
}
