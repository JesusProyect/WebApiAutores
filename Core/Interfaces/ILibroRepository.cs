using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ILibroRepository
    {
        public Task<bool> CheckLibroByIsbn(int isbn);
        public Task<bool> CheckLibroById(int id);
        public Task<Libro?> GetLibroByIsbn(int isbn);
        public Task<Libro?> GetLibroById(int id);
        public Task<List<Libro>> GetLibros();
        public Task<List<Libro>> GetLibrosByName(string name);
        public Task<int> NewLibro(Libro libro);
        public Task<int> UpdateLibro(Libro libro);
        public Task<int> Delete(Libro libro); 
    }
}
