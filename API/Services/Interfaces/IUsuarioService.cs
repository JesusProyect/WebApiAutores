using API.Dto;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace API.Services.Interfaces
{
    public interface IUsuarioService
    {
        public Task<Dictionary<int,object>> RenovarAutenticacion(string email);
        public Dictionary<int,object> GenerarHash(string textoPlano);
        public Dictionary<int, object> EncriptarTest();
        public Dictionary<int, object> EncriptarTestPorTiempo();
        public Task<IdentityUser> GetUserByEmail(string email);
        public Task<Dictionary<int, object>> HacerAdmin(string email);
        public Task<Dictionary<int, object>> RemoverAdmin(string email);
        public Task<Dictionary<int, object>> CrearUsuario(CredencialesDto credencialesDto);
        public Task<Dictionary<int, object>> Login(CredencialesDto credencialesDto);
    }
}
