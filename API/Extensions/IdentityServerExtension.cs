using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extensions
{
    public static class IdentityServerExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)  
        {
            var builder = services.AddIdentityCore<IdentityUser>();

            //EL PUTO ORDEN ES IMPORTANTE TUVE 3 HORAS PORQUE TENIA EL ADD ROLES POR ENCIMA DE EL NEW .I.I.I.I.I.I.
            builder = new IdentityBuilder(builder.UserType, builder.Services);

            builder.AddRoles<IdentityRole>();
            builder.AddEntityFrameworkStores<AppIdentityContext>();
            builder.AddSignInManager<SignInManager<IdentityUser>>();
            builder.AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer( options =>
            {
                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                  ValidateIssuer = false,
                  ValidateAudience = false,
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"]!)),
                  ClockSkew = TimeSpan.Zero
                };
            });


            services.AddAuthorization(options =>
            {
                options.AddPolicy("esAdmin", politica => politica.RequireClaim("esAdmin"));
            });

            return services;
        }

    }
}
