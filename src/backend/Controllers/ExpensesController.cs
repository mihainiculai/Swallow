using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swallow.DTOs.Itinerary;
using Swallow.Models;
using Swallow.Repositories.Interfaces;

namespace Swallow.Controllers;

[Authorize]
[Route("api/expenses")]
[ApiController]
public class ExpensesController(IExpenseRepository expenseRepository, IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddExpense([FromBody] ExpenseDto dto)
    {
        var expense = mapper.Map<Expense>(dto);
        await expenseRepository.AddAsync(expense);
        return Ok();
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateExpense([FromBody] ExpenseDto dto)
    {
        var expense = mapper.Map<Expense>(dto);
        await expenseRepository.UpdateAsync(expense);
        return Ok();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(Guid id)
    {
        await expenseRepository.DeleteAsync(id);
        return Ok();
    }
    
    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await expenseRepository.GetCategoriesAsync();
        return Ok(mapper.Map<List<ExpenseCategoryDto>>(categories));
    }
    
    [HttpGet("currencies")]
    public async Task<IActionResult> GetCurrencies()
    {
        var currencies = await expenseRepository.GetCurrenciesAsync();
        return Ok(mapper.Map<List<CurrencyDto>>(currencies));
    }
    
    [HttpGet("currency-rates")]
    public async Task<IActionResult> GetCurrencyRates()
    {
        var currencyRates = await expenseRepository.GetCurrencyRatesAsync();
        return Ok(currencyRates);
    }
}