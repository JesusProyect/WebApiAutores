using API.Dto;
using API.Dto.HATEOAS;
using API.Services.Interfaces;
using API.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Controllers.V1
{
    [Route("api/[controller]")]
    [CabeceraEstaPresenteAtributte("x-version", "1")]
    //[Route("api/v1/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly IAutorService _autorService;
        private readonly ILogger<AutorController> _loger;

        public AutorController(IAutorService autorService, ILogger<AutorController> loger, IUsuarioService usuarioService)
        {
            _autorService = autorService;
            _loger = loger;
        }

        #region GET
        [Authorize(Policy = "esAdmin")]
        [AllowAnonymous]
        [HttpGet(Name = "ObtenerAutores")]
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        //[ServiceFilter(typeof(MiFiltroDeAccion))]
        public async Task<ActionResult<List<AutorBaseDto>>> GetAutors([FromQuery] PaginacionDto paginacionDto)
        {    //esto esta paginado la version 2 no
            //throw new Exception("kkita");  prueba de excepciones con filtros globales
            var queryable = _autorService.GetAutors(); 
            await HttpContext.InsertarParametorsPaginacionEnCabecera(queryable);

            var autores = await queryable.OrderBy(autor => autor.Name).Paginar(paginacionDto).ToListAsync();

            var autoresDto =  _autorService.MapearAAutorDto(autores);

            return Ok(autoresDto);
        }


        [Authorize(Policy = "esAdmin")]
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        [HttpGet("{dni:int}", Name = "GetAutorByDni")]
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
        [HttpGet("{name}", Name = "ObtenerAutorPorNombre")]
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
        [HttpPost(Name = "CrearAutor")]
        public async Task<ActionResult> Post([FromBody] AutorPostDto autorPostDto)
        {
            Dictionary<int, object> result = await _autorService.NewAutor(autorPostDto);

            return result.Keys.First() switch
            {
                201 => CreatedAtRoute("GetAutorByDni", new { Dni = result[201].GetType().GetProperty("Dni")!.GetValue(result[201], null) }, result[201]), //como estoy devolviendo un objeto generico tengo que acceder a la propiedad de esta manera AL Id
                400 => BadRequest(result[400]),

                _ => StatusCode(StatusCodes.Status500InternalServerError, result[500])

            };

        }
        #endregion

        #region PUT

        [HttpPut("{id:int}", Name = "ActualizarAutor")]
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

        /// <summary>
        /// Borra un autor
        /// </summary>
        /// <param name="id">Id Del autor a borrar</param>
        /// <returns></returns>
        [HttpDelete("{id:int}", Name = "BorrarAutor")]
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
