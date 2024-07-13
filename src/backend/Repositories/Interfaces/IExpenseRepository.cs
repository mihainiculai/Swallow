using Swallow.DTOs.Itinerary;
using Swallow.Models;

namespace Swallow.Repositories.Interfaces;

public interface IExpenseRepository
{
    Task AddAsync(Expense expense);
    Task UpdateAsync(Expense expense);
    Task DeleteAsync(Guid id);
    Task<List<ExpenseCategory>> GetCategoriesAsync();
    Task<List<Currency>> GetCurrenciesAsync();
    Task<List<CurrencyRateDto>> GetCurrencyRatesAsync();
}