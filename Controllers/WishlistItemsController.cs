﻿using Expenses.Data;
using Expenses.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Controllers
{
    public class WishlistItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WishlistItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WishlistItems
        public async Task<IActionResult> Index()
        {
            var wishlistItems = await _context.WishlistItems.ToListAsync();
            return View(wishlistItems);
        }

        // GET: WishlistItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishlistItem = await _context.WishlistItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wishlistItem == null)
            {
                return NotFound();
            }

            return View(wishlistItem);
        }

        // GET: WishlistItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WishlistItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Category,Description,IsPurchased")] WishlistItem wishlistItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wishlistItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wishlistItem);
        }

        // GET: WishlistItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishlistItem = await _context.WishlistItems.FindAsync(id);
            if (wishlistItem == null)
            {
                return NotFound();
            }
            return View(wishlistItem);
        }

        // POST: WishlistItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Category,Description,IsPurchased")] WishlistItem wishlistItem)
        {
            if (id != wishlistItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wishlistItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WishlistItemExists(wishlistItem.Id))
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
            return View(wishlistItem);
        }

        // GET: WishlistItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishlistItem = await _context.WishlistItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wishlistItem == null)
            {
                return NotFound();
            }

            return View(wishlistItem);
        }

        // POST: WishlistItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wishlistItem = await _context.WishlistItems.FindAsync(id);
            _context.WishlistItems.Remove(wishlistItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WishlistItemExists(int id)
        {
            return _context.WishlistItems.Any(e => e.Id == id);
        }

        // POST: WishlistItems/MarkAsPurchased/5
        [HttpPost]
        public async Task<IActionResult> MarkAsPurchased(int id)
        {
            var wishlistItem = await _context.WishlistItems.FindAsync(id);
            if (wishlistItem == null)
            {
                return NotFound();
            }

            wishlistItem.IsPurchased = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
