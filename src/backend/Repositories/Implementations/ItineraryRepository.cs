using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Swallow.Data;
using Swallow.DTOs.Itinerary;
using Swallow.Exceptions.CustomExceptions;
using Swallow.Models;
using Swallow.Repositories.Interfaces;
using Swallow.Services.Itinerary;

namespace Swallow.Repositories.Implementations;


public class ItineraryRepository(
    ApplicationDbContext context,
    IUserRepository userRepository,
    IPlaceRepository placeRepository,
    IItineraryAutoCreator itineraryAutoCreator,
    IMapper mapper) : IItineraryRepository
{
    public async Task<IEnumerable<Trip>> GetUpcomingTripsAsync(User user)
    {
        var currentDate = DateOnly.FromDateTime(DateTime.Now);
        var oneDayAgo = currentDate.AddDays(-1);

        var result = await context.Trips
            .Where(t => t.UserId == user.Id && t.EndDate >= oneDayAgo && !t.IsArchived)
            .ToListAsync();

        return result;
    }
    private static void CheckValidDates(CreateItineraryDto dto)
    {
        if (dto.StartDate.AddDays(1) < DateTime.UtcNow)
            throw new BadRequestException("Start date must be in the future");
        if (dto.StartDate > dto.EndDate) throw new BadRequestException("Start date must be before end date");
    }

    private static void CheckSubscription(UserPlan currentSubscription, CreateItineraryDto dto)
    {
        if (currentSubscription.RemainingTrips <= 0) throw new BadRequestException("No remaining trips");
        if ((dto.EndDate - dto.StartDate).Days + 1 > currentSubscription.Plan.MaxTripDays)
            throw new BadRequestException("Trip duration exceeds plan limit");
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

    private async Task AddLodgingAsync(Trip trip, string placeId, string sessionToken)
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

    private async Task AddTransportAsync(Trip trip, byte transportTypeId, string placeId, string sessionToken,
        TransportRole? transportRole, DateTime? departureTime, DateTime? arrivalTime)
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

    public async Task AddAttractionAsync(Trip trip, int attractionId)
    {
        var attraction = await context.Attractions.FirstOrDefaultAsync(a => a.AttractionId == attractionId) ??
                         throw new NotFoundException("Attraction not found");
        var itineraryDay = await context.ItineraryDays.FirstOrDefaultAsync(d => d.TripId == trip.TripId) ??
                           throw new NotFoundException("Day not found");

        var itineraryAttraction = new ItineraryAttraction
        {
            ItineraryDayId = itineraryDay.ItineraryDayId,
            AttractionId = attraction.AttractionId,
            Index = itineraryDay.ItineraryAttractions.Count
        };

        await context.ItineraryAttractions.AddAsync(itineraryAttraction);
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
                new DateTime(dto.StartDate.Year, dto.StartDate.Month, dto.StartDate.Day, dto.ArrivingTime.Hour,
                    dto.ArrivingTime.Minute, 0)
            );
        }

        if (dto.DepartingPlaceId != "")
        {
            await AddTransportAsync(trip,
                (byte)dto.DepartingTransportModeId,
                dto.DepartingPlaceId,
                dto.DepartingSessionToken,
                TransportRole.Departing,
                new DateTime(dto.EndDate.Year, dto.EndDate.Month, dto.EndDate.Day, dto.DepartingTime.Hour,
                    dto.DepartingTime.Minute, 0),
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
        return await context.Trips.FirstOrDefaultAsync(t => t.TripId == guid) ??
               throw new NotFoundException("Trip not found");
    }

    public async Task ReorderAttractionAsync(Trip trip, ReorderAttractionDto dto)
    {
        trip = await context.Trips
            .Include(t => t.ItineraryDays)
            .ThenInclude(d => d.ItineraryAttractions)
            .FirstOrDefaultAsync(t => t.TripId == trip.TripId) ?? throw new NotFoundException("Trip not found");

        if (dto.SourceDay == dto.DestinationDay && dto.SourceIndex == dto.DestinationIndex) return;

        var sourceDay = trip.ItineraryDays.ElementAt(dto.SourceDay) ??
                        throw new NotFoundException("Source day not found");
        var destinationDay = trip.ItineraryDays.ElementAt(dto.DestinationDay) ??
                             throw new NotFoundException("Destination day not found");

        var sourceAttractions = sourceDay.ItineraryAttractions.OrderBy(a => a.Index).ToList();
        var destinationAttractions = destinationDay.ItineraryAttractions.OrderBy(a => a.Index).ToList();

        if (dto.SourceIndex < 0 || dto.SourceIndex >= sourceAttractions.Count)
            throw new ArgumentOutOfRangeException(nameof(dto.SourceIndex), "Invalid source index");
        if (dto.DestinationIndex < 0 || dto.DestinationIndex >= destinationAttractions.Count + 1)
            throw new ArgumentOutOfRangeException(nameof(dto.DestinationIndex), "Invalid destination index");

        var sourceAttraction = sourceAttractions.FirstOrDefault(a => a.Index == dto.SourceIndex) ??
                               throw new NotFoundException("Source attraction not found");

        foreach (var attraction in sourceAttractions.Where(attraction => attraction.Index > dto.SourceIndex))
        {
            attraction.Index--;
        }

        sourceAttractions.Remove(sourceAttraction);

        foreach (var attraction in destinationAttractions.Where(attraction => attraction.Index >= dto.DestinationIndex))
        {
            attraction.Index++;
        }

        sourceAttraction.ItineraryDayId = destinationDay.ItineraryDayId;
        sourceAttraction.Index = dto.DestinationIndex;

        destinationAttractions.Append(sourceAttraction);

        await context.SaveChangesAsync();
    }
    
    public async Task<int> DeleteAttractionAsync(Trip trip, DeleteAttractionDto dto)
    {
        trip = await context.Trips
            .Include(t => t.ItineraryDays)
            .ThenInclude(d => d.ItineraryAttractions)
            .FirstOrDefaultAsync(t => t.TripId == trip.TripId) ?? throw new NotFoundException("Trip not found");
        
        var day = trip.ItineraryDays.ElementAt(dto.Day) ?? throw new NotFoundException("Day not found");
        var attraction = day.ItineraryAttractions.FirstOrDefault(a => a.Index == dto.Index) ??
                         throw new NotFoundException("Attraction not found");
        
        var attractionId = attraction.AttractionId;
        
        context.ItineraryAttractions.Remove(attraction);
        
        foreach (var a in day.ItineraryAttractions.Where(a => a.Index > dto.Index))
        {
            a.Index--;
        }
        
        await context.SaveChangesAsync();
        
        return attractionId;
    }
    
    public async Task UpdateLodgingAsync(Trip trip, string placeId, string sessionToken)
    {
        if (trip.TripToHotel != null)
        {
            context.TripsToHotels.Remove(trip.TripToHotel);
            await context.SaveChangesAsync();
        }
        await AddLodgingAsync(trip, placeId, sessionToken);
    }
    
    public async Task ArchiveTripAsync(Trip trip)
    {
        trip.IsArchived = true;
        await context.SaveChangesAsync();
    }
    
    public async Task UnarchiveTripAsync(Trip trip)
    {
        trip.IsArchived = false;
        await context.SaveChangesAsync();
    }
    
    public async Task DeleteTripAsync(Trip trip)
    {
        foreach (var day in trip.ItineraryDays)
        {
            foreach (var attraction in day.ItineraryAttractions)
            {
                context.ItineraryAttractions.Remove(attraction);
            }
            context.ItineraryDays.Remove(day);
        }
        
        if (trip.TripToHotel != null)
        {
            context.TripsToHotels.Remove(trip.TripToHotel);
        }
        
        foreach (var transport in trip.TripTransports)
        {
            context.TripTransports.Remove(transport);
        }
        
        context.Trips.Remove(trip);
        await context.SaveChangesAsync();
    }
}