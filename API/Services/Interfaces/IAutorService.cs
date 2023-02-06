using API.Dto;
using Core.Entities;
using System.Security.Claims;

namespace API.Services.Interfaces
{
    public interface IAutorService
    {
        public Task<bool> CheckAutorById(int id);
        public Task<bool> CheckAutorByDni(int dni);
        public Task<Dictionary<int, Object >> GetAutorByDni(int dni);   //retorno el objeto si esta bien y si no un mensaje de error
        public Task<Dictionary<int,Object>> GetAutorById(int id);

        public Task<List<AutorBaseDto>> GetAutorByName(string name);

        public List<AutorBaseDto> MapearAAutorDto(List<Autor> autores);

        public IQueryable<Autor> GetAutors(); //esta es para la paginacion que no le gusta que sea asyncrono no se porque 
        

        public Task<List<AutorBaseDto>> GetAutorsAsync();//esta es para todo lo demas siu

        public Task<Dictionary<int,object>> NewAutor(AutorPostDto autorPostDto);

        public Task<Dictionary<int, string>> UpdateAutor(AutorPutDto autorPutDto, int id);

        public Task<Dictionary<int,string>> DeleteAutor(int dni);
    }
}
