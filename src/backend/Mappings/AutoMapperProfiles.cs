using AutoMapper;
using Swallow.DTOs.Attraction;
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
                .ForMember(dto => dto.Username, opt => opt.MapFrom(src => src.PublicUsername));
            CreateMap<Attraction, GetAttractionsDto>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.AttractionCategories.Select(ac => ac.AttractionCategoryId)))
                .ForMember(dest => dest.Schedules, opt => opt.MapFrom(src => src.Schedules.Select(s => new GetAttractionsDto.ScheduleDto
                {
                    WeekdayId = s.WeekdayId,
                    WeekdayName = s.Weekday.Name,
                    OpenTime = s.OpenTime,
                    CloseTime = s.CloseTime
                })));
            CreateMap<AttractionCategory, GetAttractionCategoryDto>();
        }
    }
}
