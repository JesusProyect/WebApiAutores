﻿using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters
{
    public class MiFiltroDeAccion : IActionFilter
    {
        private readonly ILogger<MiFiltroDeAccion> _logger;
        public MiFiltroDeAccion(ILogger<MiFiltroDeAccion> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Antes de la accion ");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Despues de la accion ");

        }


    }
}
