﻿using AutoMapper;
using Swallow.DTOs.Country;
using Swallow.Models.DatabaseModels;

namespace Swallow.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Country, CountryDto>().ReverseMap();
        }
    }
}