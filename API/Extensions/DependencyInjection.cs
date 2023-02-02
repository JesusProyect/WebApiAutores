using API.Services.Interfaces;
using API.Services.Services;

namespace API.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<HashService>();

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
