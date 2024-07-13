using Microsoft.EntityFrameworkCore;
using Swallow.Data;
using Swallow.DTOs.Itinerary;
using Swallow.Exceptions.CustomExceptions;
using Swallow.Models;
using Swallow.Repositories.Interfaces;

namespace Swallow.Repositories.Implementations;

public class ExpenseRepository(ApplicationDbContext context) : IExpenseRepository
{
    public async Task AddAsync(Expense expense)
    {
        await context.Expenses.AddAsync(expense);
        await context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(Expense expense)
    {
        context.Expenses.Update(expense);
        await context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var expense = await context.Expenses.FindAsync(id) ?? throw new NotFoundException("Expense not found");
        context.Expenses.Remove(expense);
        await context.SaveChangesAsync();
    }
    
    public async Task<List<ExpenseCategory>> GetCategoriesAsync()
    {
        return await context.ExpenseCategories.ToListAsync();
    }
    
    public async Task<List<Currency>> GetCurrenciesAsync()
    {
        return await context.Currencies.ToListAsync();
    }
    
    public async Task<List<CurrencyRateDto>> GetCurrencyRatesAsync()
    {
        var currencies = await GetCurrenciesAsync();
        
        List<CurrencyRateDto> currencyRates = new();
        
        foreach (var currency in currencies)
        {
            currencyRates.Add(new CurrencyRateDto
            {
                CurrencyId = currency.CurrencyId,
                RateToUSD = currency.CurrencyRates.LastOrDefault(x => x.CurrencyId == currency.CurrencyId)?.RateToUSD ?? 0
            });
        }
        
        return currencyRates;
    }
}