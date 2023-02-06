using API.Services.Interfaces;
using API.Services.Services;
using API.Utilities;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace API.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddTransient<GenerarEnlacesHATEOASService>();
            services.AddTransient<HATEOASAutorFilterAttribute>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddTransient<HashService>();

            services.AddScoped<IRootService, RootService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //services.AddHostedService<EscribirEnArchivo>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAutorService, AutorService>();
            services.AddScoped<ILibroService, LibroService>();
            services.AddScoped<IComentarioService, ComentarioService>();
            services.AddScoped<IAutorLibroService, AutorLibroService>();

            return services;

        }

    }
}
