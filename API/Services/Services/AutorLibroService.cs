using API.Services.Interfaces;
using Core.Interfaces;

namespace API.Services.Services
{
    public class AutorLibroService : IAutorLibroService
    {
        private readonly IAutorLibroRepository _autorLibroRepository;

        public AutorLibroService( IAutorLibroRepository autorLibroRepository)
        {
            _autorLibroRepository = autorLibroRepository;
        }

        public async Task<bool> CheckAutorExist(int autorId)
        {
            return await _autorLibroRepository.CheckAutorExist(autorId);
        }
    }
}
