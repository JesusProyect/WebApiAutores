using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Libro
    {
        public int Id { get; set; }
        public int Isbn { get; set; }
        public string? Title { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public List<Comentario>?  Comentarios { get; set; }
        public List<AutorLibro> AutoresLibro { get; set; } = new();

        //TODO METER UNA PROPIEDAD PUNTUACION EN LIBROS PARA EL FUTURO ESTO LUEGOOOOOOO
        //CUANDO HAGA EL GETLIBROBYISBN MOSTRAR LOS AUTORES QUE CREARON ESE LIBRO CON EL ORDEN DE PRIORIDAD LUEGOOOO PARA TERMINAR ESTE PUTO CURSO
        //GUARDAR LAS IMAGENES PARA SERVIRLAS DE LOS LIBROS




    }
}
