using AutoMapper;
using Swallow.DTOs.Itinerary;
using Swallow.DTOs.Trip;
using Swallow.Models;

namespace Swallow.Mappings;

public class ItineraryMappings : Profile
{
    public ItineraryMappings()
    {
        CreateMap<CreateItineraryDto, Trip>()
            .ForMember(trip => trip.StartDate, opt => opt.MapFrom(dto => DateOnly.FromDateTime(dto.StartDate)))
            .ForMember(trip => trip.EndDate, opt => opt.MapFrom(dto => DateOnly.FromDateTime(dto.EndDate)))
            .ForMember(trip => trip.TransportModeId, opt => opt.MapFrom(dto => dto.TransportModeId));
        CreateMap<Trip, ItineraryDto>();
        CreateMap<City, ItineraryCityDto>();
        CreateMap<TripToHotel, ItineraryTripToHotelDto>();
        CreateMap<Place, ItineraryPlaceDto>();
        CreateMap<Attraction, ItineraryPlaceDto>();
        CreateMap<Expense, ItineraryExpenseDto>();
        CreateMap<ItineraryDay, ItineraryDayDto>()
            .ForMember(dto => dto.ItineraryAttractions,
                opt => opt.MapFrom(day => day.ItineraryAttractions.OrderBy(attraction => attraction.Index)));
        CreateMap<ItineraryAttraction, ItineraryAttractionDto>();
        CreateMap<Trip, TripDto>()
            .ForMember(dto => dto.Destination, opt => opt.MapFrom(trip => trip.City.Name))
            .ForMember(dto => dto.PictureUrl, opt => opt.MapFrom(trip => trip.City.PictureUrl));
        CreateMap<Attraction, SearchAttractionResponseDto>();
        CreateMap<Expense, ExpenseDto>().ReverseMap();
        CreateMap<ExpenseCategory, ExpenseCategoryDto>();
        CreateMap<Currency, CurrencyDto>();
    }   
}