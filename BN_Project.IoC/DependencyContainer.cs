using BN_Project.Core.IService.Admin;
using BN_Project.Core.Service.Admin;
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

            services.AddScoped<IUserServices, UserService>();
            services.AddScoped<IAdminServices, AdminServices>();
            services.AddScoped<IViewRenderService, RenderViewToString>();
        }
    }
}
