using API.Dto;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly IAutorService _autorService;
        private readonly ILogger<AutorController> _loger;

        public AutorController(IAutorService autorService , ILogger<AutorController> loger)
        {
             this._autorService = autorService;
            _loger = loger;
        }

        #region GET
        [HttpGet]
        //[ServiceFilter(typeof(MiFiltroDeAccion))]
        public  async Task<ActionResult<List<AutorGetDto>>> GetAutors()
        {
            //throw new Exception("kkita");  prueba de excepciones con filtros globales
            return Ok(await _autorService.GetAutors());
        }

        
        [HttpGet("{dni:int}", Name = "GetAutorByDni")]
        public async Task<ActionResult<AutorGetByDniDto>> GetAutorByDni( int dni )
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

        [HttpGet("{name}")]
        public async Task<ActionResult<List<AutorGetDto>>> GetAutorByName(string name)
        {
            return Ok(await _autorService.GetAutorByName(name));
        }

        #endregion

        #region POST
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AutorPostDto autorPostDto)
        {
            Dictionary<int, object> result = await _autorService.NewAutor(autorPostDto);

            return result.Keys.First() switch
            {
                201 => CreatedAtRoute("GetAutorByDni", new { Dni = result[201].GetType().GetProperty("Dni")!.GetValue(result[201],null) }, result[201]), //como estoy devolviendo un objeto generico tengo que acceder a la propiedad de esta manera AL Id
                400 => BadRequest(result[400]),

                _ => StatusCode(StatusCodes.Status500InternalServerError, result[500])

            };
                
        }
        #endregion

        #region PUT

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put( AutorPutDto autorPutDto, int id )
        {
            Dictionary<int,string> result = await _autorService.UpdateAutor(autorPutDto, id);

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

        [HttpDelete("{id:int}")]
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
