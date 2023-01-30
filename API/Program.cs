using Infrastructure.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Core.Interfaces;
using API.Services.Interfaces;
using API.Services.Services;
using API.Extensions;
using Infrastructure;
using API.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using API.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(FiltroDeExcepcion));   // esto lo dejamos de momento
}).AddJsonOptions(x =>
x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
.AddNewtonsoftJson();
//si añado newtonsoft se me jode el propertyorder wtf 

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer( builder.Configuration.GetConnectionString("defaultConnection") ));

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure();

//builder.Services.AddTransient<MiFiltroDeAccion>();

//builder.Services.AddResponseCaching();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

//app.UseResponseCaching();

app.UseAuthorization();

/*app.UseEndpoints(endpoints => {
    endpoints.MapControllers();
}); //esto lo pone lo de mapcontroller lo quita pero lo voya dejar asi a ver si funciona igual
*/

app.MapControllers();

//************************************************************************

// Start the app
app.Run();
