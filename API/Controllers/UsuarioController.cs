using API.Dto;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        #region GET 

        [HttpGet("Encriptacion")]
        public ActionResult Encriptar()
        {
            return Ok(_usuarioService.EncriptarTest()[200]);
        }

        [HttpGet("EncriptacionTiempo")]
        public ActionResult EncriptarPorTiempo()
        {

            return Ok(_usuarioService.EncriptarTestPorTiempo()[200]);
        }

        [HttpGet("hash/{textoPlano}")]
        public ActionResult RealizarHash(string textoPlano)
        {
            return Ok(_usuarioService.GenerarHash(textoPlano)[200]);
        }

        [HttpGet("RenovarToken")]
        [Authorize]
        public async Task<ActionResult<AutenticacionRespuestaDto>> Renovar()    
        {
            string email = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault()!.Value;
           
            Dictionary<int, object> result = await _usuarioService.RenovarAutenticacion(email);

            return result.Keys.First() switch
            {
                200 => Ok(result[200]),
                400 => Ok(result[400]),

                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }
        #endregion

        #region POST
        
        [HttpPost("register")]
        public async Task<ActionResult<AutenticacionRespuestaDto>> Registrar(CredencialesDto credencialesDto)
        {
            Dictionary<int, object> result = await _usuarioService.CrearUsuario(credencialesDto);

            return result.Keys.First() switch
            {
                200 => Ok(result[200]),

                _ => BadRequest(result[400])
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<AutenticacionRespuestaDto>> Login( CredencialesDto credencialesDto)
        {
            Dictionary<int,object> result = await _usuarioService.Login(credencialesDto);

            return result.Keys.First() switch
            {
                200 => Ok(result[200]),
                _ => BadRequest(result[400])
            };
        }

        [Authorize(Policy = "esAdmin")]
        [HttpPost("HacerAdmin")]
        public async Task<ActionResult> HacerAdmin(EditarAdminDto editarAdminDto)
        {
            Dictionary<int, object> result = await _usuarioService.HacerAdmin(editarAdminDto.Email!);

            return result.Keys.First() switch
            {
                204 => NoContent(),
                404 => NotFound(result[404]),
                400 => BadRequest(result[400]),

                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        [Authorize(Policy = "esAdmin")]
        [HttpPost("RemoverAdmin")]
        public async Task<ActionResult> RemoverAdmin(EditarAdminDto editarAdminDto)
        {
            Dictionary<int, object> result = await _usuarioService.RemoverAdmin(editarAdminDto.Email!);

            return result.Keys.First() switch
            {
                204 => NoContent(),
                404 => NotFound(result[404]),
                400 => BadRequest(result[400]),

                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        #endregion


    }
}
