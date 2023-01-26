using Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Dto
{

    public class LibroBaseDto
    {
        [JsonPropertyOrder(2)]
        [Required]
        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe tener mas de {1} caracteres")] //EJM
        public string? Title { get; set; }

        [JsonPropertyOrder(3)]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El Campo {0} Es Requerido")]
        public int Isbn { get; set; }
    }

    public class LibroGetDto : LibroBaseDto
    {
        [JsonPropertyOrder(1)]
        public int Id { get; set; }
    }

    public class LibroGetByIsbnDto : LibroBaseDto
    {
        [JsonPropertyOrder(1)]
        public int Id { get; set; }

        [JsonPropertyOrder(10)]
        public DateTime?  FechaPublicacion { get; set; }

        [JsonPropertyOrder(11)]
        public List<AutorGetDto> Autores { get; set; } = new();

        [JsonPropertyOrder(12)]
        public List<ComentarioGetDto>? UltimosComentarios { get; set; }

    } 

    public class LibroPostDto : LibroBaseDto 
    {

        [JsonPropertyOrder(10)]
        public DateTime? FechaPublicacion { get; set; }

        [JsonPropertyOrder(11)]
        [Required]
        public List<int>?  AutoresId { get; set; }

     

    }

    public class LibroPutDto 
    {
        [Required]
        [Range(1,int.MaxValue)]
        public int Id { get; set; }

        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe tener mas de {1} caracteres")] //EJM
        public string? Title { get; set; }

        public int Isbn { get; set; }

        public DateTime? FechaPublicacion { get; set; } //TODO si no lo paso en el update se queda la que tenia antes configurar Automapper

        public List<int>? AutoresId { get; set; }
    }

    public class LibroDto
    {
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }
      
      
    }
}
