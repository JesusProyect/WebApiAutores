using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace API.Utilities
{
    public class SwaggerAgrupaPorVersion : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var nameSpaceControlador = controller.ControllerType.Namespace; //controllers.v1
            var versionApi = nameSpaceControlador!.Split('.').Last().ToLower(); // v1
            controller.ApiExplorer.GroupName = versionApi;
        }
    }
}
