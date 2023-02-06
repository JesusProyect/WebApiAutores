using API.Dto.HATEOAS;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.V1
{
    [Route("api/v1")]
    [ApiController]
    [Authorize]
    public class RootController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public RootController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet(Name = "ObtenerRoot")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DatoHATEOAS>>> Get()
        {
            List<DatoHATEOAS> datosHateoas = new()
            {
                 //esto son los links que puede ver todo el mundo
                new(enlace: Url.Link("ObtenerRoot", new { }), descripcion: "self", metodo: "GET"),
                new(enlace: Url.Link("ObtenerAutores", new { }), descripcion: "autores", metodo: "GET")
            };

            //aqui comprobamos si tiene el claim de admin y si lo tiene  le mostramos los que puede usar, no se si en el codigo esten asi la verdad pero da igual
            //si luego quiero hacer la api bien lo acomodo bonito esto es solo un ejemplo
            //TODO en los metodos meter authorice  y eso para que tenga mas sentido la app, en crear y eso que solo los que tengan token y sean admin puedan borrar usuarios y eso
            //un dasboard de administrador estaria bueno
            AuthorizationResult esAdmin = await _usuarioService.EsAdmin(User);
            if (esAdmin.Succeeded)
            {
                datosHateoas.Add(new(enlace: Url.Link("CrearAutor", new { }), descripcion: "autor-crear", metodo: "POST"));
                datosHateoas.Add(new(enlace: Url.Link("CrearLibro", new { }), descripcion: "libro-crear", metodo: "POST"));
            }

            return datosHateoas;

            /* datosHateoas.Add(new(enlace: Url.Link("GetAutorByDni", new { }), descripcion: "autores", metodo: "GET"));
             datosHateoas.Add(new(enlace: Url.Link("ObtenerAutorPorNombre", new { }), descripcion: "autores", metodo: "GET"));
             datosHateoas.Add(new(enlace: Url.Link("ActualizarAutor", new { }), descripcion: "autores", metodo: "PUT"));
             datosHateoas.Add(new(enlace: Url.Link("BorrarAutor", new { }), descripcion: "autores", metodo: "DELETE"));  */


        }

    }
}
