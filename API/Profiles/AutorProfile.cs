using API.Dto;
using AutoMapper;
using Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace API.Profiles
{
    public class AutorProfile : Profile
    {
        public AutorProfile()
        {
            CreateMap<Autor, AutorGetDto>();

            CreateMap<Autor, AutorBaseDto>();

            CreateMap<Libro, LibroGetDto>();

            CreateMap<Autor, AutorGetByDniDto>()  //los by Dni son porque tiene mas detalles le fuese puesto details pero bueno ya es tarde
                .ForMember(dest => dest.LibrosDestacados, opt => opt.MapFrom((src, dest) => 
                    (src.LibrosAutor is not null && src.LibrosAutor.Count > 0) ?  src.LibrosAutor.Select(la => la.Libro).Take(3).ToList() : new()))
                .ForMember(dest => dest.CantidadLibros, opt => opt.MapFrom((src , dest) =>
                     (src.LibrosAutor is not null) ? src.LibrosAutor.Count : 0 ));

            CreateMap<AutorPostDto, Autor>();

            CreateMap<AutorPutDto, Autor>()
                .ForMember(dest => dest.Dni, opt => opt.MapFrom((src , dest) => (src.Dni == 0) ? dest.Dni : src.Dni))
                 // .ForMember(dest => dest.Dni, opt => opt.Condition(src => (src.Dni > 0)))  Esto tambien funciona pero bueno varias maneras de tener lo mismo
                 // .ForMember(dest => dest.Name, opt => opt.Condition(src => (src.Name is not null))); asi seria uno por cada propiedad
                 .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));    //esto esta bien para los strings todos me lo hace con esto
            //pero los int como su no devuelven un valor es 0 pues no entra, o podria hacer las propiedades que acepten null o no se pero bueno lo dejo asi de momento

        }

        #region OtraFormaDeHacerElMapeoDeLibrosDestacados

        //ASI SE ENTIENDE MAS PERO AJA LAMDDA ES LAMDDA PAPA
        private static List<LibroGetDto> MapLibrosDestacados(Autor autor, AutorGetByDniDto autorDto)
        {
            List<LibroGetDto> librosDestacados = new();

            if (autor.LibrosAutor == null || autor.LibrosAutor.Count == 0) return librosDestacados;

            List<Libro> libros = autor.LibrosAutor.Select(la => la.Libro!).Take(3).ToList();

            foreach (Libro libro in libros)
            {
                librosDestacados.Add(new()
                {
                    Id = libro.Id,
                    Isbn = libro.Isbn,
                    Title = libro.Title
                });
            }

            return librosDestacados;

        }

        #endregion

    }
}
