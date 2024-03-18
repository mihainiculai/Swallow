using System.ComponentModel.DataAnnotations;

namespace Swallow.Models
{
    public class ExpenseCategory
    {
        public short ExpenseCategoryId { get; set; }
        [MaxLength(40)]
        public required string Name { get; set; }

        public virtual ICollection<Expense> Expenses { get; } = [];
    }
}
