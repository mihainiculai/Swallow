using AutoMapper;
using Swallow.DTOs.Attraction;
using Swallow.Models;

namespace Swallow.Mappings
{
    public class AttractionMappings : Profile
    {
        public AttractionMappings()
        {
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

            CreateMap<GoogleMapsDetailsResponseResult, Attraction>()
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Geometry.Location.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Geometry.Location.Longitude))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.FormattedAddress))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.InternationalPhoneNumber))
                .ForMember(dest => dest.Website, opt => opt.MapFrom(src => src.Website))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.UserRatingsTotal, opt => opt.MapFrom(src => src.UserRatingsTotal))
                .ForMember(dest => dest.GoogleMapsUrl, opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.Schedules, opt => opt.Ignore());

            CreateMap<TripAdvisorAttraction, Attraction>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Details.Description))
                .ForMember(dest => dest.TripAdvisorUrl, opt => opt.MapFrom(src => src.TripAdvisorLink))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.Details.ImageUrl))
                .ForMember(dest => dest.VisitDuration, opt => opt.MapFrom(src => src.Details.VisitDuration))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Details.Price));
        }
    }
}
