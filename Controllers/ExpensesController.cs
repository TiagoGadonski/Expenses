using Expenses.Data;
using Expenses.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpensesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Expenses
        public async Task<IActionResult> Index(string searchString, int? categoryId)
        {
            var expenses = from e in _context.Expenses.Include(e => e.Category)
                           select e;

            if (!string.IsNullOrEmpty(searchString))
            {
                expenses = expenses.Where(e => e.Description.Contains(searchString));
            }

            if (categoryId.HasValue)
            {
                expenses = expenses.Where(e => e.CategoryId == categoryId.Value);
            }

            ViewBag.Categories = new SelectList(await _context.CategoryFinances.ToListAsync(), "Id", "Name");
            ViewData["CurrentFilter"] = searchString;

            return View(await expenses.ToListAsync());
        }

        // GET: Expenses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses
                .Include(e => e.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // GET: Expenses/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.CategoryFinances, "Id", "Name");
            return View();
        }

        // POST: Expenses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Value,Date,Installments,CurrentInstallment,IsPaidThisMonth,LastPaymentDate,CategoryId")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.CategoryFinances, "Id", "Name", expense.CategoryId);
            return View(expense);
        }

        // GET: Expenses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.CategoryFinances, "Id", "Name", expense.CategoryId);
            return View(expense);
        }

        // POST: Expenses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Value,Date,Installments,CurrentInstallment,IsPaidThisMonth,LastPaymentDate,CategoryId")] Expense expense)
        {
            if (id != expense.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expense);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(expense.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.CategoryFinances, "Id", "Name", expense.CategoryId);
            return View(expense);
        }

        // GET: Expenses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses
                .Include(e => e.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.Id == id);
        }

        // POST: Expenses/MarkAsPaid/5
        [HttpPost]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            expense.IsPaidThisMonth = true;
            expense.LastPaymentDate = DateTime.Now;

            if (expense.CurrentInstallment.HasValue && expense.Installments.HasValue)
            {
                expense.CurrentInstallment++;
                if (expense.CurrentInstallment > expense.Installments)
                {
                    expense.CurrentInstallment = expense.Installments; // Prevent going over the total number of installments
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
