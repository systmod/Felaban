using AutoMapper;
using BusinessLogic.Mapping;
using BusinessLogic.Services;
using Common;
using Common.Domain.Services;
using Common.Storage.Handlers;
using DataAccess.Repositories;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace API
{
    public partial class Startup
    {
        protected void ConfigureDependency(IServiceCollection services)
        {
            services.AddScoped<DbContext, AuthContext>();
            services.AddScoped<AuthContext>();
            services.AddScoped<IAuthContextProcedures, AuthContextProcedures>();

            services.AddScoped(typeof(IEntityRepository<>), typeof(EntityRepository<>));

            // Authentication

            services.AddScoped<FileHandler>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPermissionsService, PermissionsService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IApplicationService, ApplicationService>();
            services.AddScoped<ICompanyService, CompanyService>();           

            services.AddSingleton<Common.ITools, Tools>();

            // PDF Tools
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
        }

        protected void ConfigureMapper(IServiceCollection services)
        {                        
            services.AddAutoMapper(typeof(AuthMappingProfile));
        }
    }
}
