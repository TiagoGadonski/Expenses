using Expenses.Models;

namespace Expenses.ViewModels
{
    public class HomeViewModel
    {
        public List<Expense> Expenses { get; set; }
        public List<WishlistItem> WishlistItems { get; set; }
        public decimal TotalExpenses { get; set; }
        public List<GoalProgressViewModel> GoalProgress { get; set; }
    }
}

public class GoalProgressViewModel
{
    public string Category { get; set; }
    public decimal TargetAmount { get; set; }
    public decimal TotalSpent { get; set; }
    public decimal Difference { get; set; }
}
