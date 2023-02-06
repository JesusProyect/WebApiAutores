using API.Dto.HATEOAS;
using Core.Entities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
//using API.Validations; esto es para el decoradorpersonalizado

namespace API.Dto
{

    public class AutorBaseDto : Recurso 
    {
        [JsonProperty(Order = 1)]
        public int Id { get; set; }

        [JsonProperty(Order = 10)]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe tener mas de {1} caracteres")] //EJM
        public string? Name { get; set; }

        [JsonProperty(Order = 11)]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El Campo {0} Es Requerido")]
        public int Dni { get; set; }
    }
    public class AutorGetDto : AutorBaseDto{};
    public class AutorGetByDniDto : AutorBaseDto
    {
        [JsonProperty(Order = 31)]
        public int CantidadLibros { get; set; }

        [JsonProperty(Order = 32)]
        public List<LibroGetDto> LibrosDestacados { get; set; } = new();
 
    };
    public class AutorPostDto
    {
        [JsonProperty(Order = 10)]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe tener mas de {1} caracteres")] //EJM
        public string? Name { get; set; }

        [JsonProperty(Order = 11)]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El Campo {0} Es Requerido")]
        public int Dni { get; set; }
    }
    public class AutorPutDto //los  put no heredan de Base porque los atributos son opcionales
    {
        [Required]
        [Range(1,int.MaxValue, ErrorMessage = "El Campo {0} debe tener un valor entre 1 y {2}")]
        public int Id { get; set; }

        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe tener mas de {1} caracteres")] //EJM
        public string? Name { get; set; }

        public int Dni { get; set; }
    }

    
   
    
    
    //esto era al principio ya no lo uso, no lo borro porque tiene ejemplos de validaciones
    public class AutorDto //: IValidatableObject
    {
        public int Id { get; set; }

        // [PrimeraLetraMayuscula]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe tener mas de {1} caracteres")] //EJM
        public string? Name { get; set; }

        //  [Range(18, 120)]
        // public int Edad { get; set; }

        public List<Libro>? Libros { get; set; }

        #region ValidationModel

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                var primeraLetra = Name[0].ToString();
                if (primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser mayuscula",
                        new string[] { nameof(Name) });
                }
            }
        }

        #endregion

    }


}

