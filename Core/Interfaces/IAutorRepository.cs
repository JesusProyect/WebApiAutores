using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAutorRepository
    {
        public Task<bool> CheckAutorById(int id);
        
        public Task<bool> CheckAutorByDni(int dni);

        public Task<Autor?> GetAutorById(int id);

        public Task<Autor?> GetAutorByDni(int dni);

        public Task<List<Autor>> GetAutorByName(string name);

        public Task<List<Autor>> GetAutorsAsync();
        public IQueryable<Autor> GetAutors(); // esta es para la paginacion que no le gustan las llamadas asincronas da un error raro y no se como 

        public Task<int> NewAutor(Autor autor);

        public Task<int> UpdateAutor(Autor autor);

        public Task<int> DeleteAutor(Autor autor);

    }
}
