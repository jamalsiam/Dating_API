using System.Linq;
using Api.Dtos;
using Api.Entities;
using Api.Extensions;
using AutoMapper;

namespace Api.Helpers
{
    public class AutoMapperProfiles : Profile
    {

        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
            .ForMember(
                dest => dest.PhotoUrl,
                 opt => opt.
                 MapFrom(src => src.
                 Photos.FirstOrDefault(prop => prop.IsMain).Url))
            .ForMember(
                dest => dest.Age,
                 src => src.
                 MapFrom(prop => prop.
                 DateOfBirth.CalculateAge()));

            CreateMap<Photo, PhotoDto>();

        }
    }
}
