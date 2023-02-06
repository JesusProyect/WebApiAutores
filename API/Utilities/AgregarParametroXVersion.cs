using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Utilities
{
    public class AgregarParametroXVersion  : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {

            operation.Parameters ??= new List<OpenApiParameter>(); //esto me lo sugirio el intelisense ahora me lo cargotodo seguro

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "x-version",
                In = ParameterLocation.Header,
                Required = true
            });
        }
    }
}
