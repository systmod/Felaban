using API.Filters;
using Common;
using Common.Http;
using Common.Reporting;
using ConcentratorFraud.DataAccess.Repositories;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json.Serialization;

namespace ConcentratorFraud.API
{
    public partial class Startup
    {
        protected IConfiguration Settings { get; }
        protected IWebHostEnvironment Environment { get; }

        protected string Version => Settings["Version"] ?? "1";
        protected string RoutePrefix => Settings["RoutePrefix"] ?? "";

        public Startup(IConfiguration settings, IWebHostEnvironment environment)
        {
            Settings = settings;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Add CORS support.
            // Must be first to avoid OPTIONS issues when calling from Angular/Browser
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin();
            //corsBuilder.AllowCredentials();

            services.AddCors(options =>
            {
                options.AddPolicy("allReady", corsBuilder.Build());
            });

            services.AddDbContext<ConcentradorFraudeContext>(options =>
                options.UseSqlServer(Settings.GetConnectionString("Fraud"),
                        sql => sql.EnableRetryOnFailure(3)
#if !DEBUG // Esto nos permite mejorar el rendimiento en produccion, pero en pruebas necesitamos ver errores de Entity Framework.
                                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
#endif
            ));

            ConfigureDependency(services);
            ConfigureAuth(services);
            ConfigureMapper(services);
            ConfigureSwagger(services);

            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            services.AddHeaderPropagation(options =>
            {
                options.Headers.Add("Authorization", x =>
                {
                    return x.HeaderValue;
                });
            });

            services.AddLogging();

            services.AddControllers(options =>
            {
                options.Filters.Add<HttpClientExceptionFilterAttribute>();
            }).AddJsonOptions(
                options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
            ).ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = false);

            services.AddApplicationInsightsTelemetry();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) //, IApiVersionDescriptionProvider provider)
        {
            // Put first to avoid issues with OPTIONS when calling from Angular/Browser.  
            app.UseCors("allReady");

            app.UseDeveloperExceptionPage();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                var swaggerEndpoint = $"swagger/v1/swagger.json";
                options.SwaggerEndpoint(swaggerEndpoint, $"Incidente API {Environment?.EnvironmentName} v{Version}");
                options.RoutePrefix = string.Empty;

                Console.WriteLine(Environment?.EnvironmentName);
                Console.WriteLine(swaggerEndpoint);
            });

            app.UseDefaultFiles(new DefaultFilesOptions
            {
                RequestPath = $"{RoutePrefix}/index.html"
            });

            app.UseStatusCodePagesWithReExecute($"{RoutePrefix}/error/{0}");
            app.UseExceptionHandler($"{RoutePrefix}/error");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseStatusCodePages(async context => await context.HttpContext.Response.WriteAsJsonAsync(new OperationResult((HttpStatusCode)context.HttpContext.Response.StatusCode)));
            app.UseHeaderPropagation();

            // Custom JWT AUTH middleware

            ReportClientConfig.Initialize(Settings);
            ApiControllerBase.Initialize(app.ApplicationServices, Settings, env);
            OperationExtensions.Initialize(app.ApplicationServices);

            //app.UseMiddleware<JwtMiddleware>();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
