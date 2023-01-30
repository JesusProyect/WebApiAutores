using Core.Entities;
using Core.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Kerberos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class LibroRepository : ILibroRepository
    {
        private readonly ApplicationDbContext _context;
        public LibroRepository( ApplicationDbContext applicationDbContext )
        {
            _context = applicationDbContext;
        }

        public async Task<bool> CheckLibroByIsbn(int isbn)
        {
            return await _context.Libros.AnyAsync(l => l.Isbn == isbn);
        }
        public async Task<bool> CheckLibroById(int id)
        {
            return await _context.Libros.AnyAsync(l => l.Id == id);
        }
        public async Task<Libro?> GetLibroByIsbn( int isbn )
        {
            return await _context.Libros
                .Include(l => l.Comentarios)
                .Include(l => l.AutoresLibro)
                .ThenInclude(al => al.Autor)
                .AsSplitQuery()
                .FirstOrDefaultAsync( l => l.Isbn == isbn);
        }
        public async Task<Libro?> GetLibroById(int id)
        {
            return await _context.Libros
                 .Include(l => l.AutoresLibro)
                    .ThenInclude(a => a.Autor)
                 .Include(l => l.Comentarios)
                 .AsSplitQuery()
                .FirstOrDefaultAsync(l => l.Id == id);
        }
        public async Task<List<Libro>> GetLibros()
        {
            return await _context.Libros.ToListAsync();
        }
        public async Task<List<Libro>> GetLibrosByName(string name)
        {
            return await _context.Libros
                .Where(l => l.Title!.Contains(name)).ToListAsync();
        }
        public async Task<int> NewLibro(Libro libro)
        {
            _context.Libros.Add(libro);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateLibro(Libro libro)
        {
            _context.Libros.Update(libro);// si modifico la referencia directamente lo detecta y no hay que hacer update por lo cual ESTA LINEA NO HACE FALTA LA DEJO POR NO MODIFICAR 
            return await (_context.SaveChangesAsync());
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public async Task<int> Delete(Libro libro)
        {
            _context.Libros.Remove(libro);
            return await _context.SaveChangesAsync();
        }

    }
}
