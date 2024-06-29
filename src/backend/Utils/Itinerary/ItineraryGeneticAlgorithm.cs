using Swallow.Models;

namespace Swallow.Utils.Itinerary;

public class ItineraryGeneticAlgorithm(
    List<Attraction> attractions,
    List<List<int>> distanceMatrix,
    int days,
    DateOnly firstDay,
    int firstDayMaxAttractions,
    int lastDayMaxAttractions,
    TimeOnly firstDayStartTime)
{
    private readonly Random _random = new();
    private List<List<int>> _population = [];
    
    private const int MaxAttractionsPerDay = 4;
    private const int PopulationSize = 100;
    private const int TournamentSize = 5;
    private const double MutationRate = 0.1;
    private const int MaxGenerations = 1000;

    public List<int> GenerateItinerary()
    {
        InitializePopulation();

        for (var generation = 0; generation < MaxGenerations; generation++)
        {
            var newPopulation = new List<List<int>>();

            while (newPopulation.Count < PopulationSize)
            {
                var parent1 = SelectParent();
                var parent2 = SelectParent();
                var child = Crossover(parent1, parent2);
                Mutate(child);
                newPopulation.Add(child);
            }

            _population = newPopulation;
        }

        return GetBestItinerary();
    }

    private void InitializePopulation()
    {
        for (var i = 0; i < PopulationSize; i++)
        {
            _population.Add(GenerateRandomItinerary());
        }
    }

    private List<int> GenerateRandomItinerary()
    {
        var itinerary = new List<int>();
        var availableAttractions = Enumerable.Range(1, attractions.Count).ToList();

        for (var day = 0; day < days; day++)
        {
            var maxAttractions = GetMaxAttractionsForDay(day);
            for (var i = 0; i < maxAttractions; i++)
            {
                if (availableAttractions.Count == 0) break;
                var index = _random.Next(availableAttractions.Count);
                itinerary.Add(availableAttractions[index]);
                availableAttractions.RemoveAt(index);
            }
        }

        return itinerary;
    }

    private int GetMaxAttractionsForDay(int day)
    {
        if (day == 0) return firstDayMaxAttractions;
        if (day == days - 1) return lastDayMaxAttractions;
        return MaxAttractionsPerDay;
    }

    private List<int> SelectParent()
    {
        var tournament = new List<List<int>>();
        for (var i = 0; i < TournamentSize; i++)
        {
            tournament.Add(_population[_random.Next(PopulationSize)]);
        }

        return tournament.OrderByDescending(CalculateFitness).First();
    }

    private List<int> Crossover(List<int> parent1, List<int> parent2)
    {
        var crossoverPoint = _random.Next(parent1.Count);
        var child = new List<int>(parent1.Take(crossoverPoint));

        foreach (var attraction in parent2.Where(attraction => !child.Contains(attraction)))
        {
            child.Add(attraction);
        }

        return child;
    }

    private void Mutate(List<int> itinerary)
    {
        for (var i = 0; i < itinerary.Count; i++)
        {
            if (!(_random.NextDouble() < MutationRate)) continue;
            var j = _random.Next(itinerary.Count);
            (itinerary[i], itinerary[j]) = (itinerary[j], itinerary[i]);
        }
    }

    private double CalculateFitness(List<int> itinerary)
    {
        double fitness = 0;
        var currentLocation = 0;
        var currentDate = firstDay;
        var currentTime = firstDayStartTime;
        var totalTravelTime = 0;

        for (var day = 0; day < days; day++)
        {
            var maxAttractions = GetMaxAttractionsForDay(day);
            var dayItinerary = itinerary.Skip(day * MaxAttractionsPerDay).Take(maxAttractions).ToList();

            foreach (var attraction in dayItinerary)
            {
                var travelTimeSeconds = distanceMatrix[currentLocation][attraction];
                var travelTimeMinutes = travelTimeSeconds / 60;
                currentTime = currentTime.AddMinutes(travelTimeMinutes);
                totalTravelTime += travelTimeSeconds;

                var schedule = GetScheduleForDay(attractions[attraction - 1], currentDate);
                if (schedule != null)
                {
                    if (currentTime < schedule.OpenTime)
                    {
                        currentTime = schedule.OpenTime;
                    }
                    else if (currentTime > schedule.CloseTime)
                    {
                        fitness -= 1000;
                    }
                }

                currentTime = currentTime.AddHours(2);

                currentLocation = attraction;
                fitness += 100;
            }

            var returnTimeSeconds = distanceMatrix[currentLocation][0];
            var returnTimeMinutes = returnTimeSeconds / 60;
            currentTime = currentTime.AddMinutes(returnTimeMinutes);
            totalTravelTime += returnTimeSeconds;

            if (currentTime > new TimeOnly(22, 0))
            {
                fitness -= (currentTime - new TimeOnly(22, 0)).TotalMinutes;
            }

            currentLocation = 0;
            currentDate = currentDate.AddDays(1);
            currentTime = new TimeOnly(9, 0);
        }

        fitness -= totalTravelTime / 60.0;

        return fitness;
    }

    private static Schedule? GetScheduleForDay(Attraction attraction, DateOnly date)
    {
        var weekdayId = (byte)((int)date.DayOfWeek + 1);
        return attraction.Schedules.FirstOrDefault(s => s.WeekdayId == weekdayId);
    }

    private List<int> GetBestItinerary()
    {
        return _population.OrderByDescending(CalculateFitness).First();
    }
}