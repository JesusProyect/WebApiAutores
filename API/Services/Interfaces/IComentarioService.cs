using API.Dto;
using Core.Entities;

namespace API.Services.Interfaces
{
    public interface IComentarioService
    {
        public Task<bool> CheckComentarioById(int libroId, int comentarioId);

        public Task<Dictionary<int,object>> GetComentarioById(int libroId, int comentarioId);

        public Task<Dictionary<int , List<Object>>> GetComentarios(int libroId);

        public Task<Dictionary<int, object>> NewComentario(ComentarioPostDto comentarioPostDto, int libroId);

        public Task<Dictionary<int, string>> UpdateComentario(int libroId, int comentarioId, ComentarioPutDto comentarioPutDto);

    }
}
