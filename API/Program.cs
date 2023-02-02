using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using API.Extensions;
using Infrastructure;
using API.Filters;
using Infrastructure.Identity;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
    options.Filters.Add(typeof(FiltroDeExcepcion)))   // esto lo dejamos de momento
.AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
.AddNewtonsoftJson();
//si añado newtonsoft se me jode el propertyorder wtf  las propiedades se llaman dinstinto antes era jsonpropertyorder = 1 ahora jsonProperty(order=1)

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer( builder.Configuration.GetConnectionString("defaultConnection") ));

builder.Services.AddDbContext<AppIdentityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));


builder.Services.AddIdentityServices( builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://apirequest.io")
        .AllowAnyMethod()
        .AllowAnyHeader();
        //.WithExposedHeaders();  esto es para exponer cabeceras desde el api no entendi bien pero dice que es un error comun olvidar esto asi que lo dejo por si acaso
    });
});

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure();

//builder.Services.AddTransient<MiFiltroDeAccion>();
//builder.Services.AddResponseCaching();

builder.Services.AddDataProtection();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();
var app = builder.Build();

//**************************************************************

// Configure the HTTP request pipeline.

//app.UseLoguearRespuestaHttp();

/*app.Map("/ruta1", app =>
{
    app.Run(async contexto =>
        await contexto.Response.WriteAsync("Estoy interceptando la tuberia"));
});  //ejemplo de un map con middleware  */

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors();

//app.UseResponseCaching();

app.UseAuthentication();

app.UseAuthorization();

/*app.UseEndpoints(endpoints => {
    endpoints.MapControllers();
}); //esto lo pone lo de mapcontroller lo quita pero lo voya dejar asi a ver si funciona igual
*/

app.MapControllers();

//************************************************************************

// Start the app
app.Run();
