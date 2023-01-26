using API.Dto;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/libros/{libroId:int}/[controller]")]
    [ApiController]
    public class ComentarioController : ControllerBase
    {
        private readonly IComentarioService _comentarioService;

        public ComentarioController( IComentarioService comentarioService, int libroId )
        {
            _comentarioService = comentarioService;
        }

        //no se si podria ejecutar un metodo aqui que cada vez que entre en el controlador verifique si existe el libro fuera de todo --> NO comprobado

        #region GET

        [HttpGet("{comentarioId:int}", Name = "GetComentarioById")]
        public async Task<ActionResult<ComentarioGetDto>> GetComentarioById(int libroId, int comentarioId)
        {
            //TODO VER QUE EL COMENTARIO QUE SE QUIERE MODIFICAR ES DEL LIBRO QUE SE METE
            Dictionary<int, object> result = await _comentarioService.GetComentarioById(libroId, comentarioId);

            return result.Keys.First() switch
            {
                200 => Ok(result[200]),
                400 => BadRequest(result[400]),
                404 => NotFound(result[404]),

                _ => StatusCode(StatusCodes.Status500InternalServerError)

            };
        }

        [HttpGet]
        public  async Task<ActionResult<List<ComentarioGetDto>>> Get(int libroId)
        {
            Dictionary<int , List<Object>> result = await _comentarioService.GetComentarios(libroId);   // el dto lo mapeo dentro del servicio pero no me deja castearlo aqui pero si tiene las propiedades ps

            return result.Keys.First() switch
            {
                200 => Ok(result[200]),
                404 => NotFound(result[404]),

                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        #endregion

        #region POST

        [HttpPost]
        public async Task<ActionResult> Post( int libroId, ComentarioPostDto comentarioPostDto)
        {

            Dictionary<int,object> result = await _comentarioService.NewComentario(comentarioPostDto, libroId);

            return result.Keys.First() switch
            {
                201 => CreatedAtRoute("GetComentarioById", new { libroId , comentarioId = result[201].GetType().GetProperty("Id")!.GetValue(result[201], null) },result[201]),
                400 => BadRequest(result[400]),

                _ => StatusCode(StatusCodes.Status500InternalServerError, result[500])
            };
             
        }

        #endregion

        #region PUT

        [HttpPut("{comentarioId:int}")]
        public async Task<ActionResult> UpdateComentario(int libroId, int comentarioId, ComentarioPutDto comentarioPutDto)
        {
            Dictionary<int, string> result = await _comentarioService.UpdateComentario(libroId, comentarioId, comentarioPutDto);

            return result.Keys.First() switch
            {
                204 => NoContent(),
                400 => BadRequest(result[400]),

                _ => StatusCode(StatusCodes.Status500InternalServerError, result[500])
            };
        }

        #endregion

    }
}
