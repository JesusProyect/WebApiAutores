using API.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using API.Services.Services;
using API.Dto.HATEOAS;
using System;

namespace API.Utilities
{
    public class HATEOASAutorFilterAttribute : HATEOASFiltroAttribute
    {
        private readonly GenerarEnlacesHATEOASService _generadorEnlaceService;

        public HATEOASAutorFilterAttribute(GenerarEnlacesHATEOASService generadorEnlaceHATEOASService)
        {
            _generadorEnlaceService = generadorEnlaceHATEOASService;
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var debeIncluir = DebeIncluirHATEOAS(context);

            if (!debeIncluir)
            {
                await next();
                return;
            }

            var resultado = context.Result as ObjectResult;

            var autorDto = resultado!.Value as AutorBaseDto;
            if(autorDto == null)    // si da null es porque es una lista
            {
                var autoresDto = resultado.Value as List<AutorBaseDto> ?? throw new ArgumentException("Se esperaba uan instancia de autorBaseDto o List de AutorDto");
                autoresDto.ForEach(async autor => await _generadorEnlaceService.GenerarEnlaces(autor));

                var Url = _generadorEnlaceService.ConstruirUrlHelper();
                var result = new ColeccionDeRecursos<AutorBaseDto> { Valores = autoresDto };
                result.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("ObtenerAutores", new { }), descripcion: "self", metodo: "GET"));


                if (await _generadorEnlaceService.EsAdmin()) result.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("CrearAutor", new { }), descripcion: "autor-crear", metodo: "POST"));

                resultado.Value = result;
            }
            else
            {
                await _generadorEnlaceService.GenerarEnlaces(autorDto);
            }

            await next();


        }

    }
}
