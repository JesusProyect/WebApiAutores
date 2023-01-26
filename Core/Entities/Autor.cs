using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Core.Entities
{
    public class Autor 
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Dni { get; set; }
        public List<AutorLibro> LibrosAutor { get; set; } = new();

    }
}
