using Swallow.Models;
using System.Collections.Concurrent;

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
    private const double InitialMutationRate = 0.2;
    private const double FinalMutationRate = 0.05;
    private const int MaxGenerations = 1000;
    private const int EliteSize = 2;

    public List<int> GenerateItinerary()
    {
        InitializePopulation();

        var bestFitness = double.MinValue;
        var generationsWithoutImprovement = 0;

        for (var generation = 0; generation < MaxGenerations; generation++)
        {
            var elites = _population.OrderByDescending(CalculateFitness).Take(EliteSize).ToList();
            var newPopulation = new ConcurrentBag<List<int>>(elites);

            Parallel.For(0, PopulationSize - EliteSize, _ =>
            {
                var parent1 = SelectParent();
                var parent2 = SelectParent();
                var child = Crossover(parent1, parent2);
                Mutate(child, CalculateMutationRate(generation));
                newPopulation.Add(child);
            });

            _population = newPopulation.ToList();

            var currentBestFitness = _population.Max(CalculateFitness);
            if (currentBestFitness > bestFitness)
            {
                bestFitness = currentBestFitness;
                generationsWithoutImprovement = 0;
            }
            else
            {
                generationsWithoutImprovement++;
            }

            if (generationsWithoutImprovement >= 50)
            {
                break;
            }
        }

        return GetBestItinerary();
    }

    private void InitializePopulation()
    {
        _population = Enumerable.Range(0, PopulationSize)
            .AsParallel()
            .Select(_ => GenerateSmartRandomItinerary())
            .ToList();
    }

    private List<int> GenerateSmartRandomItinerary()
    {
        var itinerary = new List<int>();
        var availableAttractions = Enumerable.Range(1, attractions.Count).ToList();
        var currentLocation = 0;

        for (var day = 0; day < days; day++)
        {
            var maxAttractions = GetMaxAttractionsForDay(day);
            for (var i = 0; i < maxAttractions; i++)
            {
                if (availableAttractions.Count == 0) break;
                
                var nextAttraction = availableAttractions.MinBy(a => distanceMatrix[currentLocation][a]);
                
                itinerary.Add(nextAttraction);
                availableAttractions.Remove(nextAttraction);
                currentLocation = nextAttraction;
            }
            currentLocation = 0; // Return to starting point
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
        return Enumerable.Range(0, TournamentSize)
            .Select(_ => _population[_random.Next(PopulationSize)])
            .OrderByDescending(CalculateFitness)
            .First();
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

    private void Mutate(List<int> itinerary, double mutationRate)
    {
        for (var i = 0; i < itinerary.Count; i++)
        {
            if (!(_random.NextDouble() < mutationRate)) continue;
            var j = _random.Next(itinerary.Count);
            (itinerary[i], itinerary[j]) = (itinerary[j], itinerary[i]);
        }
    }

    private double CalculateMutationRate(int generation)
    {
        return InitialMutationRate - (InitialMutationRate - FinalMutationRate) * generation / MaxGenerations;
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