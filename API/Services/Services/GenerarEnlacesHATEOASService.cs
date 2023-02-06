using API.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System;

namespace API.Services.Services
{
    public class GenerarEnlacesHATEOASService
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IActionContextAccessor _actionContextAccessor;

        public GenerarEnlacesHATEOASService(IAuthorizationService authorizationService,
            IHttpContextAccessor httpContextAccessor,
            IActionContextAccessor actionContextAccessor)
        {
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
            _actionContextAccessor = actionContextAccessor;
        }

        public async Task GenerarEnlaces(AutorBaseDto autorDto)
        {
            var esAdmin = await EsAdmin();
            var Url = ConstruirUrlHelper();
            autorDto.Enlaces.Add(new(enlace: Url.Link("GetAutorByDni", new { autorDto.Dni }), descripcion: "self", metodo: "GET"));
            if (esAdmin)
            {
                autorDto.Enlaces.Add(new(enlace: Url.Link("ActualizarAutor", new { id = autorDto.Id }), descripcion: "autor-actualizar", metodo: "PUT"));
                autorDto.Enlaces.Add(new(enlace: Url.Link("BorrarAutor", new { id = autorDto.Id }), descripcion: "autor-borrar", metodo: "DELETE"));
            }

        }

        public IUrlHelper ConstruirUrlHelper()
        {
            var factoria = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<IUrlHelperFactory>();
            return factoria.GetUrlHelper(_actionContextAccessor.ActionContext!);
        }

        public async Task<bool> EsAdmin()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var resultado = await _authorizationService.AuthorizeAsync(httpContext!.User, "esAdmin");

            return resultado.Succeeded;
        }
    }
}
