using Expenses.Data;
using Expenses.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var expenses = await _context.Expenses.ToListAsync();
            var totalExpenses = expenses.Sum(e => e.Value);

            var goals = await _context.ExpenseGoals.ToListAsync();
            var expensesByCategory = expenses
                .GroupBy(e => e.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalSpent = g.Sum(e => e.Value)
                }).ToList();

            var goalProgress = goals.Select(g => new GoalProgressViewModel
            {
                Category = g.Category,
                TargetAmount = g.TargetAmount,
                TotalSpent = expensesByCategory
                    .FirstOrDefault(e => e.Category == g.Category)?.TotalSpent ?? 0,
                Difference = g.TargetAmount - (expensesByCategory
                    .FirstOrDefault(e => e.Category == g.Category)?.TotalSpent ?? 0)
            }).ToList();

            var model = new HomeViewModel
            {
                Expenses = expenses,
                WishlistItems = await _context.WishlistItems.ToListAsync(),
                TotalExpenses = totalExpenses,
                GoalProgress = goalProgress
            };

            return View(model);
        }
    }
}
