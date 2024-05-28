using Expenses.Data;
using Expenses.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Controllers
{
    public class KanbanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KanbanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Kanban
        public async Task<IActionResult> Index()
        {
            ViewBag.TaskCategories = new SelectList(await _context.TaskCategories.ToListAsync(), "Id", "Name");
            ViewBag.TaskColumns = new SelectList(await _context.TaskColumns.ToListAsync(), "Id", "Name");
            var tasks = _context.TaskItems.Include(t => t.TaskCategory).Include(t => t.TaskColumn);
            return View(await tasks.ToListAsync());
        }

        // POST: Kanban/CreateCategory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory([Bind("Name")] TaskCategory taskCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Kanban/CreateColumn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateColumn([Bind("Name")] TaskColumn taskColumn)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskColumn);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Kanban/CreateTask
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask([Bind("Name,Description,TaskCategoryId,TaskColumnId,DueDate")] TaskItem taskItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
