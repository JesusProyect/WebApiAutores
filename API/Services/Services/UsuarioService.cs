using API.Dto;
using API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace API.Services.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly HashService _hashService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IDataProtector _dataProtector;

        public UsuarioService(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IMapper mapper,
            ITokenService tokenService,
            IDataProtectionProvider dataProtectionProvider,
            HashService hashService,
            IAuthorizationService authorizationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _hashService = hashService;
            _authorizationService = authorizationService;
            _dataProtector = dataProtectionProvider.CreateProtector("valor_unico_y_secreto");

        }


        public async Task<Dictionary<int,object>> CrearUsuario(CredencialesDto credencialesDto)
        {
            IdentityUser usuario = new ()
            {
                UserName = credencialesDto.Email,
                Email = credencialesDto.Email
            };

            IdentityResult result = await _userManager.CreateAsync(usuario, credencialesDto.Password);

            return result.Succeeded switch
            {
                true => new Dictionary<int, object>() { {200, _tokenService.GenerarToken(credencialesDto) } },

                false => new Dictionary<int, object>() { { 400, result.Errors} }
            };


        }

        public Dictionary<int, object> EncriptarTest()
        {
            string texto = "Jesus Noguera";
            string textoCifrado = _dataProtector.Protect(texto);

            return new() { { 200, new
                {
                    textoPlano = texto,
                    textoCifrado = textoCifrado,
                    textoDesencriptado = _dataProtector.Unprotect(textoCifrado)
                }
             }};
        }

        public Dictionary<int, object> EncriptarTestPorTiempo()
        {
            ITimeLimitedDataProtector protectorTiempoLimitado = _dataProtector.ToTimeLimitedDataProtector();
            string texto = "Jesus Noguera";
            string textoCifrado = protectorTiempoLimitado.Protect(texto, lifetime: TimeSpan.FromSeconds(5));
            //Thread.Sleep(6000); si activo esto explota porque le esoty dando duracion de5 segundos espero 6 y ya se marcho y explota

            return new() { { 200, new
                {
                    textoPlano = texto,
                    textoCifrado = textoCifrado,
                    textoDesencriptado = protectorTiempoLimitado.Unprotect(textoCifrado),
                }

             }};
        }

        public async Task<AuthorizationResult> EsAdmin(ClaimsPrincipal user) => await _authorizationService.AuthorizeAsync(user, "EsAdmin");
        

        public Dictionary<int, object> GenerarHash(string textoPlano)
        {
            return new() { { 200, new
                {
                    HashMismaPalabra1 =  _hashService.GenerateHash(textoPlano),
                    HashMismaPalabra2 = _hashService.GenerateHash(textoPlano)
                }  
            }};
        }               

        public async Task<IdentityUser> GetUserByEmail(string email)
        {
            return  await _userManager.FindByEmailAsync(email);
        }

        public async Task<Dictionary<int, object>> HacerAdmin(string email)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(email);

            if (user is null) return new() { { 404, $"Usuario No Encontrado --> Email({email})" } };

            if(_userManager.GetClaimsAsync(user).Result.Any(c => c.Type == "esAdmin")) 
                return new() { { 400, $"Este usuario ya es admin ---> Email({email})" } }; 

            return new() { { 204, await _userManager.AddClaimAsync(user, new("esAdmin", "1")) } };   // el valor en este caso da igual con tal que tenga el claim nos vale
        }

        public async Task<Dictionary<int, object>> Login(CredencialesDto credencialesDto)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(credencialesDto.Email);

            if (user is null) return new() { { 400, "Usuario o clave incorrecto" } };

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, credencialesDto.Password, lockoutOnFailure: false);

            IList<Claim> claimsDb = await _userManager.GetClaimsAsync(user);

            return result.Succeeded switch
            {
                true => new() { { 200, _tokenService.GenerarToken(credencialesDto, claimsDb) } },
                false => new() { { 400, "Login Incorrecto" } }
            };
        }

        public async Task<Dictionary<int, object>> RemoverAdmin(string email)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(email);

            if (user is null) return new() { { 404, $"Usuario No Encontrado --> Email({email})" } };

            if (!_userManager.GetClaimsAsync(user).Result.Any(c => c.Type == "esAdmin"))
                return new() { { 400, $"Este usuario no es admin ---> Email({email})" } };

            return new() { { 204, await _userManager.RemoveClaimAsync(user, new("esAdmin", "1")) } };   // el varlo en este caso da igual con tal que tenga el claim nos vale
        }

        public async Task<Dictionary<int,object>> RenovarAutenticacion(string email)
        {
            if (email is null) return new() { { 400, "Error Al Renovar el Token, Email  No puede ser null" } };

            IdentityUser user = await _userManager.FindByEmailAsync(email);
            //NO ES NECESARIO PORQUE AUNQUE SE PUEDA VER EL TOKEN NO SE PUEDEN CAMBIAR LOS VALORES SIN LA LLAVESECRETA SI LOS CAMBIAS SIN ESO NO LE VA A DEJAR ENTRAR
            //if (user is null) return new() { { 400, $"Error Al Renovar el Token, Usuario no Encontrado en la Bd ---> Email ({email})" } };

            IList<Claim> claimsDb = await _userManager.GetClaimsAsync(user);

            CredencialesDto credencialesDto = new() { Email = email };
            return new() { { 200, _tokenService.GenerarToken(credencialesDto, claimsDb) } };

        }
    }
}
