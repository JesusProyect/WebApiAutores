using API.Dto;

namespace API.Services.Interfaces
{
    public interface IAutorService
    {
        public Task<bool> CheckAutorById(int id);
        public Task<bool> CheckAutorByDni(int dni);

        public Task<Dictionary<int, Object >> GetAutorByDni(int dni);   //retorno el objeto si esta bien y si no un mensaje de error
        public Task<Dictionary<int,Object>> GetAutorById(int id);

        public Task<List<AutorGetDto>> GetAutorByName(string name);

        public Task<List<AutorGetDto>> GetAutors();

        public Task<Dictionary<int,object>> NewAutor(AutorPostDto autorPostDto);

        public Task<Dictionary<int, string>> UpdateAutor(AutorPutDto autorPutDto, int id);

        public Task<Dictionary<int,string>> DeleteAutor(int dni);
    }
}
