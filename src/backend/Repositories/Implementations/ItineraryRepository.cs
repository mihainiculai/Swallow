using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Swallow.Data;
using Swallow.DTOs.Itinerary;
using Swallow.Exceptions.CustomExceptions;
using Swallow.Models;
using Swallow.Repositories.Interfaces;
using Swallow.Services.Itinerary;

namespace Swallow.Repositories.Implementations;


public class ItineraryRepository(ApplicationDbContext context, IUserRepository userRepository, IPlaceRepository placeRepository, IItineraryAutoCreator itineraryAutoCreator, IMapper mapper) : IItineraryRepository
{
    private static void CheckValidDates(CreateItineraryDto dto)
    {
        if (dto.StartDate.AddDays(1) < DateTime.UtcNow) throw new BadRequestException("Start date must be in the future");
        if (dto.StartDate > dto.EndDate) throw new BadRequestException("Start date must be before end date");
    }
    private static void CheckSubscription(UserPlan currentSubscription, CreateItineraryDto dto)
    {
        if (currentSubscription.RemainingTrips <= 0) throw new BadRequestException("No remaining trips");
        if ((dto.EndDate - dto.StartDate).Days + 1 > currentSubscription.Plan.MaxTripDays) throw new BadRequestException("Trip duration exceeds plan limit");
    }
    
    private async Task<Trip> CreateTripAsync(User user, CreateItineraryDto dto, UserPlan currentSubscription)
    {
        var trip = mapper.Map<Trip>(dto);
        trip.UserId = user.Id;
        
        await context.Trips.AddAsync(trip);
        await context.SaveChangesAsync();
        
        await CreateItineraryDays(trip);
        
        currentSubscription.RemainingTrips--;
        await context.SaveChangesAsync();

        return trip;
    }
    
    private async Task CreateItineraryDays(Trip trip)
    {
        var days = trip.EndDate.DayNumber - trip.StartDate.DayNumber + 1;

        var itineraryDayForToAddAttractions = new ItineraryDay { TripId = trip.TripId };
        await context.ItineraryDays.AddAsync(itineraryDayForToAddAttractions);
        
        for (var i = 0; i < days; i++)
        {
            var itineraryDay = new ItineraryDay
            {
                TripId = trip.TripId,
                Date = trip.StartDate.AddDays(i)
            };
            
            await context.ItineraryDays.AddAsync(itineraryDay);
        }
        
        await context.SaveChangesAsync();
    }
    
    public async Task ClearItineraryAsync(Trip trip)
    {
        foreach (var day in trip.ItineraryDays)
        {
            foreach (var attraction in day.ItineraryAttractions)
            {
                context.ItineraryAttractions.Remove(attraction);
            }
            context.ItineraryDays.Remove(day);
        }
        
        await context.SaveChangesAsync();
        await CreateItineraryDays(trip);
    }

    public async Task AddLodgingAsync(Trip trip, string placeId, string sessionToken)
    {
        var tripToHotel = new TripToHotel
        {
            TripId = trip.TripId,
            PlaceId = (await placeRepository.GetByPlaceIdAsync(placeId, sessionToken)).PlaceId
        };
        
        await context.TripsToHotels.AddAsync(tripToHotel);
        await context.SaveChangesAsync();
        
        trip.TripToHotel = tripToHotel;
        await context.SaveChangesAsync();
    }
    
    public async Task AddTransportAsync(Trip trip, byte transportTypeId, string placeId, string sessionToken, TransportRole? transportRole, DateTime? departureTime, DateTime? arrivalTime)
    {
        var transport = new TripTransport
        {
            TripId = trip.TripId,
            TransportTypeId = transportTypeId,
            PlaceId = (await placeRepository.GetByPlaceIdAsync(placeId, sessionToken)).PlaceId,
            TransportRole = transportRole,
            DepartureTime = departureTime,
            ArrivalTime = arrivalTime
        };
        
        await context.TripTransports.AddAsync(transport);
        await context.SaveChangesAsync();
    }
    
    public async Task<Trip> CreateItineraryAsync(User user, CreateItineraryDto dto)
    {
        CheckValidDates(dto);

        var userSubscription = userRepository.GetCurrentSubscription(user);
        CheckSubscription(userSubscription, dto);
        
        var trip = await CreateTripAsync(user, dto, userSubscription);
        
        if (dto.LodgingPlaceId != "")
        {
            await AddLodgingAsync(trip, dto.LodgingPlaceId, dto.LodgingSessionToken);
        }
        
        if (dto.ArrivingPlaceId != "")
        {
            await AddTransportAsync(
                trip,
                (byte)dto.ArrivingTransportModeId,
                dto.ArrivingPlaceId,
                dto.ArrivingSessionToken,
                TransportRole.Arriving,
                null,
                new DateTime(dto.StartDate.Year, dto.StartDate.Month, dto.StartDate.Day, dto.ArrivingTime.Hour, dto.ArrivingTime.Minute, 0)
                );
        }
        
        if (dto.DepartingPlaceId != "")
        {
            await AddTransportAsync(trip, 
                (byte)dto.DepartingTransportModeId, 
                dto.DepartingPlaceId, 
                dto.DepartingSessionToken, 
                TransportRole.Departing, 
                new DateTime(dto.EndDate.Year, dto.EndDate.Month, dto.EndDate.Day, dto.DepartingTime.Hour, dto.DepartingTime.Minute, 0),
                null
                );
        }

        if (dto.ItineraryType == 1)
        {
            await itineraryAutoCreator.CreateItineraryAsync(trip);
            await context.SaveChangesAsync();
        }
        
        return trip;
    }

    public async Task<Trip> GetByIdAsync(Guid guid)
    {
        var trip = await context.Trips
            // .Include(t => t.City)
            // .Include(t => t.ItineraryDays)
            // .ThenInclude(d => d.ItineraryAttractions)
            // .Include(t => t.TripToHotel)
            // .Include(t => t.TripTransports)
            .FirstOrDefaultAsync(t => t.TripId == guid) ?? throw new NotFoundException("Trip not found");
        return trip;
    }
}