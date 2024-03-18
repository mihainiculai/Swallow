using AutoMapper;
using Swallow.DTOs.Authentication;
using Swallow.Models;

namespace Swallow.Mappings
{
    public class AuthenticationMappings : Profile
    {
        public AuthenticationMappings()
        {
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<GoogleUserInfo, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.GivenName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.FamilyName))
                .ForMember(dest => dest.GoogleProfilePictureUrl, opt => opt.MapFrom(src => src.Picture));
        }
    }
}
