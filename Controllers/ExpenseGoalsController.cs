using Expenses.Data;
using Expenses.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Controllers
{
    public class ExpenseGoalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpenseGoalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ExpenseGoals
        public async Task<IActionResult> Index()
        {
            return View(await _context.ExpenseGoals.ToListAsync());
        }

        // GET: ExpenseGoals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenseGoal = await _context.ExpenseGoals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expenseGoal == null)
            {
                return NotFound();
            }

            return View(expenseGoal);
        }

        // GET: ExpenseGoals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ExpenseGoals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Category,TargetAmount")] ExpenseGoal expenseGoal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expenseGoal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expenseGoal);
        }

        // GET: ExpenseGoals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenseGoal = await _context.ExpenseGoals.FindAsync(id);
            if (expenseGoal == null)
            {
                return NotFound();
            }
            return View(expenseGoal);
        }

        // POST: ExpenseGoals/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Category,TargetAmount")] ExpenseGoal expenseGoal)
        {
            if (id != expenseGoal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expenseGoal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseGoalExists(expenseGoal.Id))
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
            return View(expenseGoal);
        }

        // GET: ExpenseGoals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenseGoal = await _context.ExpenseGoals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expenseGoal == null)
            {
                return NotFound();
            }

            return View(expenseGoal);
        }

        // POST: ExpenseGoals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expenseGoal = await _context.ExpenseGoals.FindAsync(id);
            _context.ExpenseGoals.Remove(expenseGoal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseGoalExists(int id)
        {
            return _context.ExpenseGoals.Any(e => e.Id == id);
        }
    }
}
