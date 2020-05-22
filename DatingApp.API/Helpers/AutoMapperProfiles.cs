namespace DatingApp.API.Helpers
{
    using System.Linq;
    using AutoMapper;
    using DatingApp.API.Dtos;
    using DatingApp.API.Models;
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                // map the photo url in dto to the main image within the user object.
                .ForMember(destination => destination.PhotoUrl, option => 
                    option.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                // get the age from the datetime with extension method
                .ForMember(destination => destination.Age, option =>
                    option.MapFrom(src => src.DateOfBirth.GetAgeInYears()));
            CreateMap<User, UserForDetailedDto>()
                .ForMember(destination => destination.PhotoUrl, option => 
                    option.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(destination => destination.Age, option =>
                    option.MapFrom(src => src.DateOfBirth.GetAgeInYears()));
            CreateMap<Photo, PhotoForDetailedDto>();
            CreateMap<UserForUpdateDto, User>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();
        }
    }
}