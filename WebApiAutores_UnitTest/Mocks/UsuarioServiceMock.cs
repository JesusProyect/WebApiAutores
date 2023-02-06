using API.Dto;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebApiAutores_UnitTest.Mocks
{
    public class UsuarioServiceMock : IUsuarioService
    {

        public AuthorizationResult? Resultado { get; set; }
        public Task<Dictionary<int, object>> CrearUsuario(CredencialesDto credencialesDto)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, object> EncriptarTest()
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, object> EncriptarTestPorTiempo()
        {
            throw new NotImplementedException();
        }

        public Task<AuthorizationResult> EsAdmin(ClaimsPrincipal user)
        {
            return Task.FromResult(Resultado!);
        }

        public Dictionary<int, object> GenerarHash(string textoPlano)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUser> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<int, object>> HacerAdmin(string email)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<int, object>> Login(CredencialesDto credencialesDto)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<int, object>> RemoverAdmin(string email)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<int, object>> RenovarAutenticacion(string email)
        {
            throw new NotImplementedException();
        }
    }
}
