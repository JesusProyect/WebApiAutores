using API.Dto;
using API.Dto.HATEOAS;
using API.Services.Interfaces;
using API.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers.V2
{
    [Route("api/[controller]")]
    [CabeceraEstaPresenteAtributte("x-version","2")]
    //[Route("api/v2/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly IAutorService _autorService;
        private readonly ILogger<AutorController> _loger;
        private readonly IUsuarioService _usuarioService;

        public AutorController(IAutorService autorService, ILogger<AutorController> loger, IUsuarioService usuarioService)
        {
            _autorService = autorService;
            _loger = loger;
            _usuarioService = usuarioService;
        }

        #region GET
        [Authorize(Policy = "esAdmin")]
        [AllowAnonymous]
        [HttpGet(Name = "ObtenerAutoresv2")]
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        //[ServiceFilter(typeof(MiFiltroDeAccion))]
        public async Task<ActionResult<List<AutorBaseDto>>> GetAutors()
        {
            //throw new Exception("kkita");  prueba de excepciones con filtros globales

            List<AutorBaseDto> autoresDto = await _autorService.GetAutorsAsync();
            autoresDto.ForEach(autor => autor.Name = autor.Name!.ToUpper());
            return Ok(autoresDto);
        }


        [Authorize(Policy = "esAdmin")]
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        [HttpGet("{dni:int}", Name = "GetAutorByDniv2")]
        public async Task<ActionResult<AutorGetByDniDto>> GetAutorByDni(int dni)
        {
            _loger.LogInformation("Obtenemos los autores");// esto deberia ir en el servicio y es un ejemplo del curso
            Dictionary<int, object> result = await _autorService.GetAutorByDni(dni);

            return result.Keys.First() switch
            {
                200 => Ok(result[200]),
                404 => NotFound(result[404]),

                _ => StatusCode(StatusCodes.Status500InternalServerError, result[500])
            };
        }

        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        [Authorize(Policy = "esAdmin")]
        [AllowAnonymous]
        [HttpGet("{name}", Name = "ObtenerAutorPorNombrev2")]
        public async Task<ActionResult<List<AutorGetDto>>> GetAutorByName(string name)
        {
            List<AutorBaseDto> coincidencias = await _autorService.GetAutorByName(name);
            return Ok(coincidencias);
        }

        #endregion

        #region POST
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        [Authorize(Policy = "esAdmin")]
        [AllowAnonymous]
        [HttpPost(Name = "CrearAutorv2")]
        public async Task<ActionResult> Post([FromBody] AutorPostDto autorPostDto)
        {
            Dictionary<int, object> result = await _autorService.NewAutor(autorPostDto);

            return result.Keys.First() switch
            {
                201 => CreatedAtRoute("GetAutorByDniv2", new { Dni = result[201].GetType().GetProperty("Dni")!.GetValue(result[201], null) }, result[201]), //como estoy devolviendo un objeto generico tengo que acceder a la propiedad de esta manera AL Id
                400 => BadRequest(result[400]),

                _ => StatusCode(StatusCodes.Status500InternalServerError, result[500])

            };

        }
        #endregion

        #region PUT

        [HttpPut("{id:int}", Name = "ActualizarAutorv2")]
        public async Task<ActionResult> Put(AutorPutDto autorPutDto, int id)
        {
            Dictionary<int, string> result = await _autorService.UpdateAutor(autorPutDto, id);

            return result.Keys.First() switch
            {
                204 => NoContent(),
                400 => BadRequest(result[400]),
                404 => NotFound(result[404]),

                _ => StatusCode(StatusCodes.Status500InternalServerError, result[500])
            };
        }

        #endregion

        #region DELETE

        [HttpDelete("{id:int}", Name = "BorrarAutorv2")]
        public async Task<ActionResult> Delete(int id)
        {
            Dictionary<int, string> result = await _autorService.DeleteAutor(id);

            return result.Keys.First() switch
            {
                200 => Ok(result[200]),
                400 => BadRequest(result[400]),

                _ => StatusCode(StatusCodes.Status500InternalServerError, result[500])
            };

        }

        #endregion



    }
}
