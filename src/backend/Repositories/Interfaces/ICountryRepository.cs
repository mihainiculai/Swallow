﻿using Swallow.Models;

namespace Swallow.Repositories.Interfaces
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetAllAsync();
        Task<Country> GetByIdAsync(short id);
    }
}
