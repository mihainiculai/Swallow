﻿using AutoMapper;
using Swallow.DTOs.City;
using Swallow.DTOs.Country;
using Swallow.DTOs.Destination;
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
            CreateMap<City, CityDiscoverDto>().ReverseMap();
            CreateMap<Country, CountryDetailsDto>().ReverseMap();
            CreateMap<City, CitySearchDto>()
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.Name))
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.Country.Iso2));
            CreateMap<City, DestinationDto>()
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.Name))
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.Country.Iso2));
        }
    }
}
