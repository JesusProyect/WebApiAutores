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
    public class AutorLibroRepository : IAutorLibroRepository
    {
        private readonly ApplicationDbContext _context;

        public AutorLibroRepository( ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<bool> CheckAutorExist(int autorId)
        {
            return _context.AutoresLibros.AnyAsync(al => al.AutorId == autorId); 
        }
    }
}
