using API.Dto;
using AutoMapper;
using Core.Entities;

namespace API.Profiles
{
    public class ComentarioProfile : Profile
    {
        public ComentarioProfile()
        {
            CreateMap<ComentarioPostDto, Comentario>();

            CreateMap<Comentario, ComentarioGetDto>();

            CreateMap<ComentarioPutDto, Comentario>()
                .ForMember(dest => dest.Contenido, opt => opt.Condition((src, dest) => src.Contenido != null));

        }

    }
}
