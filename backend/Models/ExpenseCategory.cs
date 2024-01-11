namespace Swallow.Models.DatabaseModels
{
    public class ExpenseCategory
    {
        public short ExpenseCategoryId { get; set; }
        public required string Name { get; set; }

        public virtual ICollection<Expense> Expenses { get; } = new List<Expense>();
    }
}
