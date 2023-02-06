using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace API.Utilities
{
    public class CabeceraEstaPresenteAtributte : Attribute, IActionConstraint
    {
        private readonly string _cabecera;
        private readonly string _valor;

        public int Order => 0;

        public CabeceraEstaPresenteAtributte(string cabecera, string valor)
        {
            _cabecera = cabecera;
            _valor = valor;
        }

        public bool Accept(ActionConstraintContext context)
        {
            var cabeceras = context.RouteContext.HttpContext.Request.Headers;
            if (!cabeceras.ContainsKey(_cabecera)) return false;

            return string.Equals(cabeceras[_cabecera], _valor, StringComparison.OrdinalIgnoreCase);
        }
    }
}
