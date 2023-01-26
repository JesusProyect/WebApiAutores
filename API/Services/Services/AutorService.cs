using API.Dto;
using API.Services.Interfaces;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Kerberos;
using System.Diagnostics;
using System.Net;

namespace API.Services.Services
{
    public class AutorService : IAutorService
    {
        private readonly IAutorLibroService _autorLibroService;
        private readonly IAutorRepository _autorRepository;
        private readonly IMapper _mapper;
        public AutorService(IAutorRepository autorRepository, IMapper mapper, IAutorLibroService autorLibroService)
        {
            _autorRepository = autorRepository;
            _mapper = mapper;
            _autorLibroService = autorLibroService;
        }
        
        public async Task<bool> CheckAutorById(int id)
        {
            return await _autorRepository.CheckAutorById(id);
        }

        public async Task<bool> CheckAutorByDni(int dni)
        {
            return await _autorRepository.CheckAutorById(dni);
        }

        public async Task<Dictionary<int , Object>> GetAutorById(int id)
        {
            Autor? autor = await _autorRepository.GetAutorById(id);
            return autor switch
            {
                null => new() { { 404, $"El Id dado no existe en la Bd --> ({id})" } },
                _ => new() { { 200, _mapper.Map<AutorGetDto>(autor) } }
            };
        }

        public async Task<Dictionary<int,Object>> GetAutorByDni(int dni)
        {
            if (dni <= 0) return new() { { 400, "El Dni debe ser un numero mayor a 0" } };

            Autor? autor = await _autorRepository.GetAutorByDni(dni);

            return autor switch
            {
                null => new () { { 404, $"El Dni Dado No existe en la base de datos --> ({dni})" } },
                _ => new() { { 200, _mapper.Map<AutorGetByDniDto>(autor) } }
            };
                
        }

        public async Task<List<AutorGetDto>> GetAutorByName(string name)
        {
            return  _mapper.Map<List<AutorGetDto>>( await _autorRepository.GetAutorByName(name));
        }
           
        public async Task<List<AutorGetDto>> GetAutors()
        {
            List<Autor> autores = await _autorRepository.GetAutors();
            return autores.Count == 0 
                ? new() 
                : _mapper.Map<List<Autor> , List<AutorGetDto>>(autores);
        }

        public async Task<Dictionary<int,object>> NewAutor(AutorPostDto autorPostDto)
        {
            if(await _autorRepository.CheckAutorByDni(autorPostDto.Dni)) return new() { { 400, $"El Dni Suministrado ya se encuetra registrado en la Bd --> ({autorPostDto.Dni})" } };
            
            Autor autor = _mapper.Map<AutorPostDto , Autor> (autorPostDto);

            int result = await _autorRepository.NewAutor(autor);
           
            return result switch
            {
                > 0 => new() { { 201, _mapper.Map<AutorGetDto>(autor) } },

                _ => new() { { 500, "Error Inesperado" } }

            };
            
        }

        public async Task<Dictionary<int , string>> UpdateAutor(AutorPutDto autorPutDto, int id)
        {
            #region Validations

            if (autorPutDto.Id <= 0 || id <= 0) return new() { { 400, "Los Id proporcionados deben ser numeros positivos " } };

            if (autorPutDto.Id != id) return new(){ { 400 , $"Los Id No coinciden entre si --> ({autorPutDto.Id} != {id})" } };  //los id deben ser iguales  
            
            if (!await _autorRepository.CheckAutorById(id)) return new() { { 404 , $"El id proporcionado no Existe en la Bd --> ({id})  "} };  // el id ingresado debe existir enla bd
            
            Autor? autor = await _autorRepository.GetAutorById(id);   // como ya seque existe me lo traigo

            //si es distinto el dni de la bd al del objeto que mete el usuario lo ha cambiado tengo que comprobar si no coincide con algun otro
            if ( autor!.Dni != autorPutDto.Dni)  
            {
                if (await _autorRepository.CheckAutorByDni(autorPutDto.Dni)) return new() { { 400, $"El Nuevo Dni registrado ya existe en la Bd --> ({autorPutDto.Dni})"} };
            }

            #endregion

            autor = _mapper.Map(autorPutDto, autor);

            return await _autorRepository.UpdateAutor( autor! ) > 0 
                ? new() { { 204, "" } } //204 ES NOCONTENT SE RETORNAR AL ACTUALIZAR EXITOSAMENTE
                : new() { { 500, "Error inesperado" } };
        }

        public async Task<Dictionary<int,string>> DeleteAutor(int id)
        {
            if (id <= 0) return new() { { 400, "El Id debe ser un numero mayor a 0" } };

            if (!await _autorRepository.CheckAutorById(id)) return new() { { 400, "Id No encontrado" } };

            if (await _autorLibroService.CheckAutorExist(id)) return new() { { 400, "El Autor Tiene algunos libros registrados, para poder borrar el autor, primero borre sus libros" } };
          
            Autor? autor = await _autorRepository.GetAutorById(id);

            return await _autorRepository.DeleteAutor(autor!) switch
            {
                > 0 => new() { { 200, "Eliminado" } },

                _ => new() { { 500, "Error Inesperado" } }
            };
        }

    }
}

