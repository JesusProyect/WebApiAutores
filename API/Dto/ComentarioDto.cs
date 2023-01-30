using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace API.Dto
{
    public class ComentarioBaseDto
    {
        [JsonProperty(Order = 2)]
        [Required]
        public string? Contenido { get; set; }
    }

    public class ComentarioGetDto : ComentarioBaseDto
    {
        [JsonProperty(Order = 1)]
        public int Id { get; set; } 
    }

    public class ComentarioPostDto : ComentarioBaseDto
    {

    }

    public class ComentarioPutDto
    {
        [Required]
        public int Id { get; set; }
        public string? Contenido { get; set; }
    }

    //TODO hay que poner el Id de la peersona que hizo el comentario para que si luego quiere editarlo pueda hacerlo. para luego para el portafolio
}
