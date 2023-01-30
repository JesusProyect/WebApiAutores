using API.Dto;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore.Design;

namespace API.Profiles
{
    public class LibroProfile : Profile
    {
        public LibroProfile()
        {
            CreateMap<Libro, LibroGetDto>();

            CreateMap<Libro, LibroGetByIsbnDto>()
                .ForMember(dest => dest.UltimosComentarios, opt => opt.MapFrom(MapCommentsPreview))
                .ForMember(dest => dest.Autores, opt => opt.MapFrom((src , dest) =>
                    (src.AutoresLibro.Count > 0) 
                    ? src.AutoresLibro.OrderBy(al => al.Orden)
                        .Select(al => al.Autor)
                        .ToList() 
                    : new() ));   

            CreateMap<LibroPostDto, Libro>()
                .ForMember(dest => dest.AutoresLibro, opt => opt.MapFrom(MapAutoresLibroPost));

            CreateMap<LibroPutDto, Libro>()
                 .ForMember(dest => dest.Isbn, opt => opt.MapFrom(KeepIsbnIfSrcProvidesZero))
                 .ForMember(dest => dest.AutoresLibro, opt => opt.Ignore())
                    .AfterMap((src , dest) => dest.AutoresLibro = MapAutoresLibroPut(src, dest))
                 .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<LibroPatchDto, Libro>()
                .ForMember(dest => dest.AutoresLibro, opt => opt.MapFrom(MapAutoresLibroPatch));
            
            CreateMap<Libro, LibroPatchDto>()
                .ForMember(dest => dest.AutoresId, opt => opt.MapFrom((src, dest ) => src.AutoresLibro.Select(al => al.Autor).Select(a => a!.Id)));

        }

        private int KeepIsbnIfSrcProvidesZero(LibroPutDto libroPutDto, Libro libro)
        {
           return libroPutDto.Isbn == 0
                ? libro.Isbn
                :libroPutDto.Isbn;
        }
        private List<Comentario> MapCommentsPreview(Libro libro, LibroGetByIsbnDto libroGetByIsbnDto)
        {
            List<Comentario> comentariosPreview = new();

            if(libro.Comentarios is not null) comentariosPreview =  libro.Comentarios.OrderByDescending(c => c.Id).Take(3).ToList();

            return comentariosPreview;
            
        }
        private List<AutorLibro> MapAutoresLibroPost(LibroPostDto libroPostDto, Libro libro)
        {
            var result = new List<AutorLibro>();
            for(int i = 0; i < libroPostDto.AutoresId!.Count; i++)
            {
                result.Add(new()
                {
                    AutorId = libroPostDto.AutoresId[i],
                    Orden = (i + 1)
                });
            }

            return result;
        }
        private static List<AutorLibro> MapAutoresLibroPut(LibroPutDto libroPutDto, Libro libro)
        {
            List<AutorLibro> result = libro.AutoresLibro!;

            if (libroPutDto.AutoresId == null || libroPutDto.AutoresId.Count == 0) return result;

            result = new();

            for (int i = 0; i < libroPutDto.AutoresId!.Count; i++)
            {
                result.Add(new()
                {
                    AutorId = libroPutDto.AutoresId[i],
                    Orden = (i + 1)
                });
            }

            return result;
        }

        private static List<AutorLibro> MapAutoresLibroPatch(LibroPatchDto libroPatchDto, Libro libro)
        {
            List<AutorLibro> result = libro.AutoresLibro!;

            if (libroPatchDto.AutoresId == null || libroPatchDto.AutoresId.Count == 0) return result;

            result = new();

            for (int i = 0; i < libroPatchDto.AutoresId!.Count; i++)
            {
                result.Add(new()
                {
                    AutorId = libroPatchDto.AutoresId[i],
                    Orden = (i + 1)
                });
            }

            return result;
        }
    }

}