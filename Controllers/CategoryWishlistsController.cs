using Expenses.Data;
using Expenses.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Controllers
{
    public class CategoryWishlistsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryWishlistsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CategoryWishlists
        public async Task<IActionResult> Index()
        {
            return View(await _context.CategoryWishlists.ToListAsync());
        }

        // GET: CategoryWishlists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryWishlist = await _context.CategoryWishlists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryWishlist == null)
            {
                return NotFound();
            }

            return View(categoryWishlist);
        }

        // GET: CategoryWishlists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoryWishlists/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] CategoryWishlist categoryWishlist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryWishlist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoryWishlist);
        }

        // GET: CategoryWishlists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryWishlist = await _context.CategoryWishlists.FindAsync(id);
            if (categoryWishlist == null)
            {
                return NotFound();
            }
            return View(categoryWishlist);
        }

        // POST: CategoryWishlists/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] CategoryWishlist categoryWishlist)
        {
            if (id != categoryWishlist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryWishlist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryWishlistExists(categoryWishlist.Id))
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
            return View(categoryWishlist);
        }

        // GET: CategoryWishlists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryWishlist = await _context.CategoryWishlists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryWishlist == null)
            {
                return NotFound();
            }

            return View(categoryWishlist);
        }

        // POST: CategoryWishlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoryWishlist = await _context.CategoryWishlists.FindAsync(id);
            if (categoryWishlist != null)
            {
                _context.CategoryWishlists.Remove(categoryWishlist);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryWishlistExists(int id)
        {
            return _context.CategoryWishlists.Any(e => e.Id == id);
        }
    }
}
