using API.Dto;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public  AutenticacionRespuestaDto GenerarToken(CredencialesDto credencialesDto, IList<Claim>? claimsDb = null)
        {
            List<Claim> claims = new()
            {
                new("email", credencialesDto.Email!)
                //new(ClaimTypes.Email, credencialesDto.Email!)  Esto es otra forma de hacerlo pero asi es mas sencillo como arriba

            };

            if(claimsDb is not null) claims.AddRange(claimsDb);

            SymmetricSecurityKey key = new (Encoding.UTF8.GetBytes(_config["Token:Key"]!));

            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            DateTime expirarion = DateTime.Now.AddDays(1);

            JwtSecurityToken securityToken = new(issuer: null, audience: null, claims, expires: expirarion, signingCredentials: creds);

            return new ()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expirarion
            };

        }
    }
}
