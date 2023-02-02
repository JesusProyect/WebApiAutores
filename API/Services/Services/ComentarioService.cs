using API.Dto;
using API.Services.Interfaces;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace API.Services.Services
{
    public class ComentarioService : IComentarioService
    {
        private readonly IComentarioRepository _comentarioRepository;
        private readonly ILibroService _libroService;
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public ComentarioService( IComentarioRepository comentarioRepository, ILibroService libroService, IUsuarioService usuarioService, IMapper mapper )
        {
            _comentarioRepository = comentarioRepository;
            _libroService = libroService;
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        public async Task<Dictionary<int, object>> GetComentarioById(int libroId, int comentarioId)
        {
               
            if (!await _libroService.CheckLibroById(libroId)) return new() { { 400, $"El Libro indicado no Existe ---> ({libroId})" } };

            if (!await _comentarioRepository.CheckComentarioById(libroId, comentarioId)) return new() { { 404, $"No Existe el Id de comentario --> ({comentarioId}) en el libro Id ---> ({libroId})" } };

            Comentario? comentario = await _comentarioRepository.GetComentarioById(comentarioId);

            return new() { { 200, _mapper.Map<ComentarioGetDto>(comentario) } };
           
        }

        public async Task<bool> CheckComentarioById(int libroId , int comentarioId)
        {
            return await _comentarioRepository.CheckComentarioById(libroId, comentarioId);
        }

        public async Task<Dictionary<int, List<Object>>> GetComentarios(int libroId)
        {
            if(!await _libroService.CheckLibroById(libroId)) return new() { { 404, new() { $"El Libro con el Id Proporcionado no Existe  ---> ({libroId})" } } };

            return new() { { 200, new(_mapper.Map<List<ComentarioGetDto>>(await _comentarioRepository.GetComentarios(libroId))) } };

        }
        
        public async Task<Dictionary<int, object>> NewComentario( ComentarioPostDto comentarioPostDto, int libroId, string userEmail)
        {
            if (userEmail is null) return new() { { 400, "El Email de Usuario no puede ser nulo" } };

            IdentityUser user = await _usuarioService.GetUserByEmail(userEmail);

            if (user is null) return new() { { 400, $"El Email de Usuario no existe ---> ({userEmail})" } };

            if ( !await _libroService.CheckLibroById(libroId)) return new() { { 400, $"El Id de libro proporcionado no se encuentra en la base de datos -->({libroId})"} };

            Comentario comentario = _mapper.Map<Comentario>(comentarioPostDto);
            comentario.LibroId = libroId;
            comentario.UsuarioId = user.Id;

            return await _comentarioRepository.NewComentario(comentario) switch
            {
               > 0 => new(){ { 201, _mapper.Map<ComentarioGetDto>(comentario) } },
               _ => new() { { 500, "Error Inesperado"} }
            };
        }

        public async Task<Dictionary<int, string>> UpdateComentario(int libroId, int comentarioId, ComentarioPutDto comentarioPutDto)
        {
            #region Validations

            if (libroId <= 0 || comentarioId <= 0) return new() { { 400, $"Los Id especificados deben ser numeros positivos" } };

            if (comentarioPutDto.Id != comentarioId) return new() { { 400, "Los  Id no coinciden" } };

            if (!await _libroService.CheckLibroById(libroId)) return new() { { 400, $"El Libro Especificado no existe ---> ({libroId})" } };

            if (!await _comentarioRepository.CheckComentarioById(libroId, comentarioId)) return new() { { 400, $"No existe el Id de comentario --> ({comentarioId}) para el libro --> ({libroId})" } };

            #endregion

            Comentario? comentario = await _comentarioRepository.GetComentarioById(comentarioId);

            comentario = _mapper.Map(comentarioPutDto, comentario);

            return await _comentarioRepository.UpdateComentario(comentario!) switch
            {
                > 0 => new() { { 204, "" } },

                _ => new() { { 500, "Error Inesperado" } }
            };
        }
    }
}
