using AutoMapper;
using Common.Domain.Services;
using Common;
using DataAccess.Repositories;
using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.EntityFrameworkCore;
using Common.Storage.Handlers;
using ConcentratorFraud.Domain.Services;
using ConcentratorFraud.BusinessLogic.Services;
using ConcentratorFraud.BusinessLogic.Mapping;
using ConcentratorFraud.DataAccess.Repositories;

namespace ConcentratorFraud.API
{
    public partial class Startup
    {
        protected void ConfigureDependency(IServiceCollection services)
        {
            services.AddScoped<DbContext, ConcentradorFraudeContext>();
            services.AddScoped<ConcentradorFraudeContext>();
            //services.AddScoped<IAuthContextProcedures, AuthContextProcedures>();

            services.AddScoped(typeof(IEntityRepository<>), typeof(EntityRepository<>));

            // Authentication

            services.AddScoped<FileHandler>();           
            services.AddScoped<IIncidentService, IncidentService>();
            services.AddScoped<ICatalogoService, CatalogoService>();

            services.AddSingleton<Common.ITools, Tools>();

            // PDF Tools
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
        }

        protected void ConfigureMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(FraudMappingProfile));
        }
    }
}
