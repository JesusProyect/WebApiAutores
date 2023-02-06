using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace API.Dto.HATEOAS
{
    public class Recurso
    {
        
        [JsonProperty(Order = 50)]
        public List<DatoHATEOAS> Enlaces { get; set; } = new();


        //esto es de newtonsoft lo ameeeeeeeeee ignora cosas al serializar
        public bool ShouldSerializeEnlaces()
        {
            return (Enlaces.Count > 0);
        }

    }
}
