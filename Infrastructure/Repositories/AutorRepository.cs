using Core.Entities;
using Core.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AutorRepository : IAutorRepository
    {
        private readonly ApplicationDbContext _context;
        public AutorRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<bool> CheckAutorById(int id)
        {
            return await _context.Autores.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> CheckAutorByDni(int dni)
        {
            return await _context.Autores.AnyAsync(a => a.Dni == dni);
        }    //esto es para validar el post porque va sin ID porque se crea solo, valido contra el DNI

        public async Task<Autor?> GetAutorById(int id)
        {
            return await _context.Autores
                .FirstOrDefaultAsync(a => a.Id== id);
        }

        public async Task<Autor?> GetAutorByDni( int dni )
        {
            return await _context.Autores
                .Include(a => a.LibrosAutor)
                .ThenInclude(la => la.Libro)  //solo los primeros tres para hacer como un perfil del autor y mostrar los libros mas votados
                .AsSplitQuery()
                .FirstOrDefaultAsync( a => a.Dni == dni );
        }

        public async Task<List<Autor>> GetAutorByName(string name)
        {
            return await _context.Autores
                .Where(a => a.Name!.Contains(name)).ToListAsync();
        }

        public  async Task<List<Autor>> GetAutors()
        {
            return await _context.Autores
                .ToListAsync();
        }

        public async Task<int> NewAutor(Autor autor)
        { 
             _context.Autores.Add(autor);
             return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAutor(Autor autor)
        {
            _context.Autores.Update(autor);
             
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAutor(Autor autor)
        {
            _context.Autores.Remove(autor);
            return await _context.SaveChangesAsync();
        }
    }
}
