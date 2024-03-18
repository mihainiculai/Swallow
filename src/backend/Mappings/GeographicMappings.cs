using AutoMapper;
using Swallow.DTOs.City;
using Swallow.DTOs.Country;
using Swallow.Models;

namespace Swallow.Mappings
{
    public class GeographicMappings : Profile
    {
        public GeographicMappings()
        {
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<City, CountryCityDto>().ReverseMap();
            CreateMap<City, CityDto>().ReverseMap();
        }
    }
}
