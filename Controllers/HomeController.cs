using Expenses.Data;
using Expenses.Models;
using Expenses.ViewModel;
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
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var expenses = await _context.Expenses
                .Include(e => e.Category)
                .Where(e => (e.LastPaymentDate == null || e.LastPaymentDate.Value.Month != currentMonth || e.LastPaymentDate.Value.Year != currentYear) || (e.Installments != null && e.CurrentInstallment < e.Installments))
                .ToListAsync();

            var wishlistItems = await _context.WishlistItems.Include(w => w.Category).ToListAsync();

            var viewModel = new HomeViewModel
            {
                Expenses = expenses ?? new List<Expense>(),
                WishlistItems = wishlistItems ?? new List<WishlistItem>()
            };

            return View(viewModel);
        }
    }
}
