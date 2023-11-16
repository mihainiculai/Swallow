using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Swallow.Models.DatabaseModels
{
    public class Expense
    {
        public int ExpenseId { get; set; }
        public short ExpenseCategoryId { get; set; }
        public virtual ExpenseCategory ExpenseCategory { get; set; } = null!;
        public int TripId { get; set; }
        public virtual Trip Trip { get; set; } = null!;
        [MaxLength(50)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? AttachmentURL { get; set; }

        [Precision(10, 2)]
        public decimal? Price { get; set; }
        public short? CurrencyId { get; set; }
        public virtual Currency? Currency { get; set; } = null!;
    }
}
