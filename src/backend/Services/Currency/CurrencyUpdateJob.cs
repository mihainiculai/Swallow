namespace Swallow.Services.Currency;

public class CurrencyUpdateJob(IServiceProvider serviceProvider)
{
    public async Task ExecuteAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var currencyUpdater = scope.ServiceProvider.GetRequiredService<ICurrencyUpdater>();
        await currencyUpdater.UpdateCurrenciesAsync();
    }
}