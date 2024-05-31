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
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsCategory> NewsCategories { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<TaskCategory> TaskCategories { get; set; }
        public DbSet<TaskColumn> TaskColumns { get; set; }
        public DbSet<CryptoPrice> CryptoPrices { get; set; }
        public DbSet<CryptoNews> CryptoNews { get; set; }
        public DbSet<CryptoFeedback> CryptoFeedbacks { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
