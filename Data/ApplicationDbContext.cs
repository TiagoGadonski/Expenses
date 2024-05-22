using Expenses.Models;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<CategoryFinance> CategoryFinances { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<CategoryWishlist> CategoryWishlists { get; set; }
        public DbSet<ExpenseGoal> ExpenseGoals { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
