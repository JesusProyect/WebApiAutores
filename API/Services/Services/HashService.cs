using API.Dto;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace API.Services.Services
{
    public class HashService
    {
        public ResultadoHashDto GenerateHash(string textoPlano)
        {
            //Absolutely, this generate the salt and the hash one generates the hash but i keep this name because te both funtion work together and return hash
            byte[] salt = new byte[16];
            using var random = RandomNumberGenerator.Create();
            random.GetBytes(salt);

            return Hash(textoPlano, salt);
        }

        public ResultadoHashDto Hash(string textoPlano, byte[] salt)
        {
            var llaveDerivada = KeyDerivation.Pbkdf2(
                password: textoPlano,
                salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 32);

            var hash = Convert.ToBase64String(llaveDerivada);

            return new()
            {
                Hash = hash,
                Salt = salt
            };
        }
    }
}
