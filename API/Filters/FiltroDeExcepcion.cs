using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters
{
    public class FiltroDeExcepcion : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionFilterAttribute> _logger;

        public FiltroDeExcepcion(ILogger<ExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogError("Exception: {Exception} , \n message: {Message}", context.Exception, context.Exception.Message);
                
            base.OnException(context);
        }
    }
}
