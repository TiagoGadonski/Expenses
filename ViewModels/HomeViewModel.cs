using Expenses.Models;

namespace Expenses.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<Expense> Expenses { get; set; }
        public IEnumerable<WishlistItem> WishlistItems { get; set; }
    }
}
