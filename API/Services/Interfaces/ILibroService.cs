using API.Dto;
using Core.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Services.Interfaces
{
    public interface ILibroService
    {
        public Task<bool> CheckLibroByIsbn(int isbn);
        public Task<bool> CheckLibroById(int id);
        public bool CheckAutoresIguales(List<int> autoresId);
        public Task<int> CheckAutoresExisten(List<int> autoresId);
        public Task<Dictionary<int,object>> GetLibroByIsbn(int isbn);
        public Task<List<LibroGetDto>> GetLibros();
        public Task<List<LibroGetDto>> GetLibrosByName(string name);
        public Task<Dictionary<int,object>> NewLibro(LibroPostDto libroPostDto);
        public Task<Dictionary<int, string>> UpdateLibro(LibroPutDto libroPutDto, int isbn);
        public Task<Dictionary<int, object>> ValidatePatchLibroDto(int id, JsonPatchDocument<LibroPatchDto> patchDocument);
        public Task<Dictionary<int, object>> PatchLibro(Object DtoYLibro); // este objeto contiene 2 propiedades 1 con el libro y otra con el dto, esta un poco feo lo se :(
        public Task<Dictionary<int, string>> Delete(int id);

    }
}
