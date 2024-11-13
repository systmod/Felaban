using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API
{
    public partial class Startup
    {

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureSwagger(IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);

            //services.AddApiVersioning(options =>
            //{
            //    options.DefaultApiVersion = new ApiVersion(1, 0);
            //    options.AssumeDefaultVersionWhenUnspecified = true;
            //    options.ReportApiVersions = true;
            //});

            //services.AddVersionedApiExplorer(options =>
            //{
            //    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service  
            //    // note: the specified format code will format the version as "'v'major[.minor][-status]"  
            //    options.GroupNameFormat = "'v'VVV";

            //    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat  
            //    // can also be used to control the format of the API version in route templates  
            //    options.SubstituteApiVersionInUrl = true;
            //});


            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Auth API for Apps",
                    Version = "v1"
                });

                //swagger.SwaggerDoc("v2", new OpenApiInfo
                //{
                //    Title = "AUTH API for Admins",
                //    Version = "v2"
                //}); 

                // To Enable authorization using Swagger (JWT)    
                swagger.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
                {
                    Name = "ApiKey",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "ApiKey",
                    BearerFormat = "Token",
                    In = ParameterLocation.Header,
                    Description = "Escriba por favor la clave de acceso del cliente.",
                });

                // To Enable authorization using Swagger (JWT)    
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Escriba 'Bearer' [espacio] y luego agrege un token válido en la entrada de texto siguiente.\r\n\r\nEjemplo: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "ApiKey"
                            }
                        },
                        new string[] {}
                    }
                });

            });
        }
    }


}
