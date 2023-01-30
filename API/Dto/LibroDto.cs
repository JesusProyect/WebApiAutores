using Core.Entities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Dto
{

    public class LibroBaseDto
    {
        [JsonProperty(Order = 2)]
        [Required]
        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe tener mas de {1} caracteres")] //EJM
        public string? Title { get; set; }

        [JsonProperty(Order = 3)]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El Campo {0} Es Requerido")]
        public int Isbn { get; set; }
    }

    public class LibroGetDto : LibroBaseDto
    {
        [JsonProperty(Order = 1)]
        public int Id { get; set; }
    }

    public class LibroGetByIsbnDto : LibroBaseDto
    {
        [JsonProperty(Order = 1)]
        public int Id { get; set; }

        [JsonProperty(Order = 10)]
        public DateTime?  FechaPublicacion { get; set; }

        [JsonProperty(Order = 11)]
        public List<AutorGetDto> Autores { get; set; } = new();

        [JsonProperty(Order = 12)]
        public List<ComentarioGetDto>? UltimosComentarios { get; set; }

    } 

    public class LibroPostDto : LibroBaseDto 
    {

        [JsonProperty(Order = 10)]
        public DateTime? FechaPublicacion { get; set; }

        [JsonProperty(Order = 11)]
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

        public DateTime? FechaPublicacion { get; set; } 

        public List<int>? AutoresId { get; set; }
    }

    public class LibroPatchDto
    {
        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe tener mas de {1} caracteres")] //EJM
        public string? Title { get; set; }

        public int Isbn { get; set; }

        public DateTime? FechaPublicacion { get; set; }

        public List<int>? AutoresId { get; set; }
    }

    public class LibroDto
    {
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }
      
      
    }
}
