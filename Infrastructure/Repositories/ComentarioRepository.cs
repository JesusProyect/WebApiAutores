using Core.Entities;
using Core.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ComentarioRepository : IComentarioRepository
    {
        private readonly ApplicationDbContext _context;

        public ComentarioRepository( ApplicationDbContext context )
        {
            _context = context;
        }

        public async Task<bool> CheckComentarioById(int libroId, int comentarioId)
        {
            return await _context.Comentarios.AnyAsync(c => c.Id == comentarioId && c.LibroId == libroId);
        }  //en el check hago la validacion del libro, por lo cual no lo hago en el get, pero siempre tengo que llamar el check

        public async Task<List<Comentario>> GetComentarios(int libroId)
        {
            return await _context.Comentarios
                   .Where(c => c.LibroId == libroId).ToListAsync();
        }

        public async Task<Comentario?> GetComentarioById(int id)
        {
            return await _context.Comentarios.FirstOrDefaultAsync(c => c.Id == id );
        }

        public async Task<int> NewComentario(Comentario comentario)
        {
            _context.Comentarios.Add(comentario);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateComentario(Comentario comentario)
        {
            _context.Comentarios.Update(comentario);

            return await _context.SaveChangesAsync();
           
        }
    }


}
