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

            CreateMap<Photo, PhotoDto>().ReverseMap();
            CreateMap<MemberUpdateDto, AppUser>().ReverseMap();
            CreateMap<UserFollow, FollowAddDto>().ReverseMap();
            CreateMap<UserFollow, FollowReadDto>().ReverseMap();
            CreateMap<UserFollow, FollowDeleteDto>().ReverseMap();
            CreateMap<FollowReadDto, FollowAddDto>().ReverseMap();

            CreateMap<Post, PostUpdateDto>().ReverseMap()
                .ForMember(dest => dest.AppUserId, opt => opt.MapFrom(src => src.UserId));

            CreateMap<Post, PostReadDto>().ReverseMap()
           .ForMember(dest => dest.AppUserId, opt => opt.MapFrom(src => src.UserId));
            CreateMap<Post, PostAddDto>().ReverseMap()
            .ForMember(dest => dest.AppUserId, opt => opt.MapFrom(src => src.UserId));

        }
    }
}
