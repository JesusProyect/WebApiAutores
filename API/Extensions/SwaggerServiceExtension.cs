using API.Utilities;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace API.Extensions
{
    public static class SwaggerServiceExtension
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "WebApiAutores",
                    Version = "v1",
                    Description = "Api Curso de Udemy",
                    Contact = new OpenApiContact
                    {
                        Email = "test@test.com",
                        Name = "Jesus Noguera",
                        Url = new Uri("http://voilaa.tk")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT"
                    }
                 });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "WebApiAutores", Version = "v2" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header

                });

                c.OperationFilter<AgregarParametroHATEOAS>();
                c.OperationFilter<AgregarParametroXVersion>();

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                         Array.Empty<string>() 
                    }
                });

                var archivoXml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var rutaXml = Path.Combine(AppContext.BaseDirectory, archivoXml);

                c.IncludeXmlComments(rutaXml);

            });

            return services;
        }
    }
}
