using API.Dto;
using API.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        private readonly ILibroService _libroService;

        public LibroController(ILibroService libroService)
        {
            _libroService = libroService;

        }

        #region GET
        [HttpGet("{isbn:int}", Name = "GetLibroByIsbn")]
        public async Task<ActionResult<LibroGetByIsbnDto>> GetLibroByIsbn(int isbn)
        {
            Dictionary<int, object> result = await _libroService.GetLibroByIsbn(isbn);

            return result.Keys.First() switch
            {
                404 => NotFound(result[404]),
                200 => Ok(result[200]),

                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };


        }

        [HttpGet(Name = "ObtenerLibros")]
        public async Task<ActionResult<List<LibroGetDto>>> GetLibros()
        {
            return await _libroService.GetLibros();
        }

        [HttpGet("{name}", Name = "GetLibrosByName")]
        public async Task<ActionResult<List<LibroGetDto>>> GetLibrosByName(string name)
        {
            return Ok(await _libroService.GetLibrosByName(name));

        }

        #endregion

        #region POST
        [HttpPost(Name = "CrearLibro")]
        public async Task<ActionResult> Post(LibroPostDto libroPostDto)
        {

            Dictionary<int, object> result = await _libroService.NewLibro(libroPostDto);

            return result.Keys.First() switch
            {
                201 => CreatedAtRoute("GetLibroByIsbn", new { Isbn = result[201].GetType().GetProperty("Isbn")!.GetValue(result[201], null) }, result[201]),
                400 => BadRequest(result[400]),

                _ => StatusCode(StatusCodes.Status500InternalServerError, result[500])

            };

        }

        #endregion

        #region PUT
        [HttpPut("{id:int}", Name = "ActualizarLibro")]
        public async Task<ActionResult> Put(LibroPutDto libroPutDto, int id)
        {

            Dictionary<int, string> result = await _libroService.UpdateLibro(libroPutDto, id);
            return result.Keys.First() switch
            {
                200 => Ok(result[200]),
                400 => BadRequest(result[400]),

                _ => StatusCode(StatusCodes.Status500InternalServerError, result[500])
            };


        }

        #endregion

        #region PATCH

        //TODO ACOMODAR EL 500 QUE DEVUELVE PORQUE SI MANDAMOS UN PARAMETRO EXACTAMENTE IGUAL NO HACE EL SAVECHANGES Y DEVUELVE 0 Y YO ESTOY DEVOLVIENDO ERROR 500 POR ESO, NO ESTA BIEN
        [HttpPatch("{id:int}", Name = "ActualizarLibroPatch")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDto> patchDocument)
        {

            Dictionary<int, object> result = await _libroService.ValidatePatchLibroDto(id, patchDocument);

            if (result.Keys.First() == 400) return BadRequest(result[400]);
            if (result.Keys.First() == 404) return NotFound(result[404]);

            LibroPatchDto libroPatchDto = (LibroPatchDto)result[200].GetType().GetProperty("Dto")!.GetValue(result[200], null)!;

            patchDocument.ApplyTo(libroPatchDto, ModelState);

            if (!TryValidateModel(libroPatchDto)) return BadRequest(ModelState);

            result = await _libroService.PatchLibro(result[200]);

            return result.Keys.First() switch
            {
                200 => NoContent(),

                _ => StatusCode(StatusCodes.Status500InternalServerError, result[500])
            };






        }

        #endregion

        #region DELETE
        [HttpDelete("{id:int}", Name = "BorrarLibro")]
        public async Task<ActionResult> Delete(int id)
        {
            Dictionary<int, string> result = await _libroService.Delete(id);

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
