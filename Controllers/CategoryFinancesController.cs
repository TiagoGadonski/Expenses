using Expenses.Data;
using Expenses.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Controllers
{
    public class CategoryFinancesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryFinancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CategoryFinances
        public async Task<IActionResult> Index()
        {
            return View(await _context.CategoryFinances.ToListAsync());
        }

        // GET: CategoryFinances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryFinance = await _context.CategoryFinances
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryFinance == null)
            {
                return NotFound();
            }

            return View(categoryFinance);
        }

        // GET: CategoryFinances/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoryFinances/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] CategoryFinance categoryFinance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryFinance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoryFinance);
        }

        // GET: CategoryFinances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryFinance = await _context.CategoryFinances.FindAsync(id);
            if (categoryFinance == null)
            {
                return NotFound();
            }
            return View(categoryFinance);
        }

        // POST: CategoryFinances/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] CategoryFinance categoryFinance)
        {
            if (id != categoryFinance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryFinance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryFinanceExists(categoryFinance.Id))
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
            return View(categoryFinance);
        }

        // GET: CategoryFinances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryFinance = await _context.CategoryFinances
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryFinance == null)
            {
                return NotFound();
            }

            return View(categoryFinance);
        }

        // POST: CategoryFinances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoryFinance = await _context.CategoryFinances.FindAsync(id);
            if (categoryFinance != null)
            {
                _context.CategoryFinances.Remove(categoryFinance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryFinanceExists(int id)
        {
            return _context.CategoryFinances.Any(e => e.Id == id);
        }
    }
}
