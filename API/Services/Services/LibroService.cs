using API.Dto;
using API.Services.Interfaces;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Services.Services
{
    public class LibroService : ILibroService
    {

        private readonly ILibroRepository _libroRepository;
        private readonly IMapper _mapper;
        private readonly IAutorService _autorService;

        public LibroService( ILibroRepository libroRepository, IMapper mapper, IAutorService autorService)
        {
            _libroRepository = libroRepository;
            _mapper = mapper;
            _autorService = autorService;
        }

        public async Task<bool> CheckLibroById(int id)
        {
            return await _libroRepository.CheckLibroById(id);
        }

        public async Task<bool> CheckLibroByIsbn(int isbn)
        {
            return await _libroRepository.CheckLibroByIsbn(isbn);
        }

        public bool CheckAutoresIguales( List<int> autoresId)
        { 
            return autoresId.Distinct().Count() != autoresId.Count;//si es distinto quiere decir que alguno se repite
        }

        public async Task<int> CheckAutoresExisten( List<int> autoresId)
        {
            List<int> autores =  _autorService.GetAutorsAsync().Result
                .Where(a => autoresId.Contains(a.Id))
                .Select(aut => aut.Id)
                .ToList();

            if (autores.Count != autoresId.Count)
                return  await Task.FromResult(autoresId.Where(a => !autores.Contains(a)).First());  //retorno el id que falta

            return await Task.FromResult(-1); //significa que esta ok
        }

        public async Task<Dictionary<int,object>> GetLibroByIsbn(int isbn)
        {
            Libro? libro = await _libroRepository.GetLibroByIsbn(isbn);

            return libro switch
            {
                null => new() { { 404, "El Isbn proporcionado no corresponde con un libro de la Bd" } },

                _ => new() { { 200, _mapper.Map<LibroGetByIsbnDto>(libro) } }

            };
        }

        public async Task<List<LibroGetDto>> GetLibros()
        {
            List<Libro> libros = await _libroRepository.GetLibros();
            return _mapper.Map<List<LibroGetDto>>(libros);
        }

        public async Task<List<LibroGetDto>> GetLibrosByName(string name)
        {
            List<Libro> libros = await _libroRepository.GetLibrosByName(name);

            return _mapper.Map<List<LibroGetDto>>(libros);
          
        }

        public async Task<Dictionary<int,object>> NewLibro(LibroPostDto libroPostDto)
        {
            #region Validations

            if (libroPostDto.AutoresId is null || libroPostDto.AutoresId.Count == 0) return new() { { 400, "Debe Especificar Al menos un valor para el Id del Autor"  } };

            if (libroPostDto.AutoresId.Any(a => a <= 0)) return new() { { 400, "El Id de los autores debe ser un numero mayor a 0"  } };

            if (CheckAutoresIguales(libroPostDto.AutoresId)) return new() { { 400, "Si el libro Posee mas de un autor, Los Id de los mismos debe ser Distintos entre ellos"  } };

            int result = await CheckAutoresExisten(libroPostDto.AutoresId);
            if (result != -1) return new() { { 400, $"El Id del Autor Especificado no Existe ({result})" } };
                
            if (await CheckLibroByIsbn(libroPostDto.Isbn)) return new() { { 400, $"El Isbn Ingresado ya se encuentra registrado ---> ({libroPostDto.Isbn})" } };

            #endregion

            Libro libro = _mapper.Map<LibroPostDto, Libro>(libroPostDto);

            return await _libroRepository.NewLibro(libro) switch
            {
                > 0 => new() { { 201, _mapper.Map<LibroGetDto>(libro) } },

                _ => new() { { 500, "Error Inesperado" } }
            };
        }

        public async Task<Dictionary<int,string>> UpdateLibro(LibroPutDto libroPutDto, int id)
        {
            #region Validations

            if (libroPutDto.Id <= 0 || id <= 0) return new() { { 400, "Los Id proporcionados deben ser numeros positivos" } };

            if (libroPutDto.Id != id) return new() { { 400, "Los Id Ingresados no coinciden entre si" } };

            Libro? libro = await _libroRepository.GetLibroById(id);
            if (libro == null) return new() { { 400, $"El Id proporcionado no se encuentra en la Base de Datos ---> ({id})" } };

            if (libroPutDto.AutoresId is not null && libroPutDto.AutoresId.Count > 0)
            {
                if (libroPutDto.AutoresId.Any(a => a <= 0)) return new() { { 400, "El Id de los autores debe ser un numero mayor a 0" } };
                if (CheckAutoresIguales(libroPutDto.AutoresId)) return new() { { 400, "Si el libro Posee mas de un autor, Los Id de los mismos debe ser Distintos entre ellos" } };
                int result = await CheckAutoresExisten(libroPutDto.AutoresId);
                if (result != -1) return new() { { 400, $"El Id del Autor Especificado no Existe ({result})" } };

            }

            if(libroPutDto.Isbn is not 0 && libroPutDto.Isbn != libro.Isbn)
            {
                if (await CheckLibroByIsbn(libroPutDto.Isbn)) return new() { { 400, $"El Isbn Ingresado ya se encuentra registrado ---> ({libroPutDto.Isbn})" } };

            }

            #endregion

            libro = _mapper.Map<LibroPutDto, Libro>(libroPutDto,libro);

            return await _libroRepository.UpdateLibro(libro) switch
            {
                > 0 => new() { { 200, "Actualizado" } },

                _ => new() { { 500, "Error Inesperado" } }
            };
        }

        public async Task<Dictionary<int, object>> ValidatePatchLibroDto(int id, JsonPatchDocument<LibroPatchDto> patchDocument)
        {
            //tengo que hacerlo asi porque el modelState y el TryValidate son metodos de la clase
            //abstracta ControllerBase por lo cual no puedo tenerlo aqui, a menos que derive de controller base pero no se que tan ideal seria eso
            //por lo cual tendre que tener algo de logica en mis controladores :(
            //con este hago las primeras validaciones y luego le hago path en el controlador y en el otro metodo si ya el savechanges
            if (id <= 0 ) return new() { { 400, "Los Id proporcionados deben ser numeros positivos" } };

            if (patchDocument is null) return new() { { 400, "Error en formato PATCH" } };

            Libro? libro = await _libroRepository.GetLibroById(id);

            if (libro is null) return new() { { 404, $"Libro No Encontrado Id ---> ({id})" } };

            //retorno un objeto con dos propiedades el Dto mapeado del libro y el libro para luego mandarlo a otro metodo a que se actualice
            //ahora en el controlador lo valido el Dto contra el model State y luego lo updateo es una movida nome gusta
            return new() { { 200, new { Dto = _mapper.Map<LibroPatchDto>(libro) , Libro = libro} } };
        }  

        public async Task<Dictionary<int,object>> PatchLibro(Object DtoYLibro)
        {
            LibroPatchDto libroPatchDto = (LibroPatchDto)DtoYLibro.GetType().GetProperty("Dto")!.GetValue(DtoYLibro, null)!;
            Libro libro = (Libro)DtoYLibro.GetType().GetProperty("Libro")!.GetValue(DtoYLibro, null)!;

            #region Validaiton

            if (libroPatchDto.AutoresId is not null && libroPatchDto.AutoresId.Count > 0)
            {
                if (libroPatchDto.AutoresId.Any(a => a <= 0)) return new() { { 400, "El Id de los autores debe ser un numero mayor a 0" } };
                if (CheckAutoresIguales(libroPatchDto.AutoresId)) return new() { { 400, "Si el libro Posee mas de un autor, Los Id de los mismos debe ser Distintos entre ellos" } };
                int result = await CheckAutoresExisten(libroPatchDto.AutoresId);
                if (result != -1) return new() { { 400, $"El Id del Autor Especificado no Existe ({result})" } };

            }

            if (libroPatchDto.Isbn != 0 && libroPatchDto.Isbn != libro.Isbn)
            {
                if (await CheckLibroByIsbn(libroPatchDto.Isbn)) return new() { { 400, $"El Isbn Ingresado ya se encuentra registrado ---> ({libroPatchDto.Isbn})" } };

            }

            #endregion

            _mapper.Map(libroPatchDto, libro);

            return await _libroRepository.SaveChangesAsync() switch
            {
                >= 0 => new() { { 200, "Actualizado" } },  //siempre va a entrar en esta linea a menos que se rompa la bd algo asi pero aja era la forma mas facil de solucionar
                //si los cambios ingresados son identicos devuelve 0 por lo cual me devolviera el de abajo un error si no es asi es que no ha modificado nada
                //antes estaba > 0 pero lo cambie a >= 0 

                _ => new() { { 500, "Error Inesperado al intentar actualizar con patch" } }
            };


        }

        public async Task<Dictionary<int, string>> Delete(int id)
        {
            Libro? libro = await _libroRepository.GetLibroById(id);

            if (libro is null) return new() { { 400, $"El Id proporcionado no Existe ---> ({id})" } };
             
            return await _libroRepository.Delete(libro) switch
            {
                > 0 => new() { { 200, "Borrado"} },

                _ => new() { { 500, "Error Inesperado"} }

            };
        }

    }
}
