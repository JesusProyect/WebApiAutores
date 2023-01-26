using System.ComponentModel.DataAnnotations;

namespace API.Dto
{
    public class ComentarioBaseDto
    {
        [Required]
        public string? Contenido { get; set; }
    }

    public class ComentarioGetDto : ComentarioBaseDto
    {
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
