using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Utilities
{
    public class AgregarParametroHATEOAS : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if(context.ApiDescription.HttpMethod != "GET" && context.ApiDescription.HttpMethod != "POST")  return;

            operation.Parameters ??= new List<OpenApiParameter>(); //esto me lo sugirio el intelisense ahora me lo cargotodo seguro

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "incluirHATEOAS",
                In = ParameterLocation.Header,
                Required = false
            });
        }
    }
}
