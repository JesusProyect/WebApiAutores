using API.Dto;
using System.Security.Claims;

namespace API.Services.Interfaces
{
    public interface ITokenService
    {
        public AutenticacionRespuestaDto GenerarToken(CredencialesDto credencialesDto, IList<Claim>? claimsDb = null);
    }
}
