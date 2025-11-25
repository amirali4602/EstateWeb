using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Estate.DataAccess.Data;
using Estate.Models;
using Estate.Utility;
using Microsoft.AspNetCore.Authorization;

namespace EstateWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class ToiletsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ToiletsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Toilets
        public async Task<IActionResult> Index()
        {
            return View(await _context.Toilet.ToListAsync());
        }

        // GET: Toilets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toilet = await _context.Toilet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toilet == null)
            {
                return NotFound();
            }

            return View(toilet);
        }

        // GET: Toilets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Toilets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Toilet toilet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(toilet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(toilet);
        }

        // GET: Toilets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toilet = await _context.Toilet.FindAsync(id);
            if (toilet == null)
            {
                return NotFound();
            }
            return View(toilet);
        }

        // POST: Toilets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Toilet toilet)
        {
            if (id != toilet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toilet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToiletExists(toilet.Id))
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
            return View(toilet);
        }

        // GET: Toilets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toilet = await _context.Toilet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toilet == null)
            {
                return NotFound();
            }

            return View(toilet);
        }

        // POST: Toilets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toilet = await _context.Toilet.FindAsync(id);
            if (toilet != null)
            {
                _context.Toilet.Remove(toilet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToiletExists(int id)
        {
            return _context.Toilet.Any(e => e.Id == id);
        }
    }
}
