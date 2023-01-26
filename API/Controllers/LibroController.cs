using API.Dto;
using API.Services.Interfaces;
using API.Services.Services;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Kerberos;

namespace API.Controllers
{
    [Route("api/[controller]")]
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
            Dictionary<int,object> result = await _libroService.GetLibroByIsbn(isbn);

            return result.Keys.First() switch
            {
                404 => NotFound(result[404]),
                200 => Ok(result[200]),

                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };

           
        }

        [HttpGet]
        public async Task<ActionResult<List<LibroGetDto>>> GetLibros()
        {
            return await _libroService.GetLibros();
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<List<LibroGetDto>>> GetLibrosByName(string name)
        {
             return Ok(await _libroService.GetLibrosByName(name));

        }

        #endregion

        #region POST
        [HttpPost]
        public async Task<ActionResult> Post(LibroPostDto libroPostDto)
        {

            Dictionary<int,object> result = await _libroService.NewLibro(libroPostDto);

            return result.Keys.First() switch
            {
                201 => CreatedAtRoute("GetLibroByIsbn", new { Isbn = result[201].GetType().GetProperty("Isbn")!.GetValue(result[201], null) }, result[201]),
                400 => BadRequest(result[400]),

                _ => StatusCode(StatusCodes.Status500InternalServerError, result[500])

            }; 
                    
        }

        #endregion

        #region PUT
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(LibroPutDto libroPutDto, int id)
        {
        
            Dictionary<int,string> result = await _libroService.UpdateLibro(libroPutDto, id);
            return result.Keys.First() switch
            {
                200 => Ok(result[200]),
                400 => BadRequest(result[400]),

                _ => StatusCode(StatusCodes.Status500InternalServerError, result[500])
            };
                

        }

        #endregion

        #region DELETE
        [HttpDelete("{id:int}")]
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
