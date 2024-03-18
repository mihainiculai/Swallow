using Microsoft.EntityFrameworkCore;

namespace Swallow.Models
{
    public class CurrencyRate
    {
        public int CurrencyRateId { get; set; }
        public short CurrencyId { get; set; }
        public virtual Currency Currency { get; set; } = null!;

        [Precision(12, 6)]
        public decimal RateToUSD { get; set; }
        public DateTime Date { get; set; }
    }
}
