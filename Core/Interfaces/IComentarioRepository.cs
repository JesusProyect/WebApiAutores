using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IComentarioRepository
    {
        public Task<bool> CheckComentarioById(int libroId, int comentarioId);
        public Task<Comentario?> GetComentarioById(int id);
        public Task<List<Comentario>> GetComentarios(int libroId);
        public Task<int> NewComentario(Comentario comentario);
        public Task<int> UpdateComentario(Comentario comentario);
    }
}
