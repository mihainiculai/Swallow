using AutoMapper;
using Swallow.DTOs.Authentication;
using Swallow.DTOs.City;
using Swallow.DTOs.Country;
using Swallow.Models.DatabaseModels;
using Swallow.Utils.AttractionDataProviders;

namespace Swallow.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<City, CountryCityDto>().ReverseMap();
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<User, UserDto>()
                .ForMember(dto => dto.Username, opt => opt.MapFrom(src => src.PublicUsername))
                .ForMember(dto => dto.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dto => dto.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dto => dto.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dto => dto.ProfilePictureURL, opt => opt.MapFrom(src => src.ProfilePictureURL))
                .ForMember(dto => dto.Public, opt => opt.MapFrom(src => src.Public));
        }
    }
}
