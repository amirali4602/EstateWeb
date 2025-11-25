using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Estate.DataAccess.Data;
using Estate.Models;
using Microsoft.AspNetCore.Authorization;
using Estate.Utility;

namespace EstateWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class HeatingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HeatingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Heatings
        public async Task<IActionResult> Index()
        {
            return View(await _context.Heatings.ToListAsync());
        }

        // GET: Heatings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var heating = await _context.Heatings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (heating == null)
            {
                return NotFound();
            }

            return View(heating);
        }

        // GET: Heatings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Heatings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Heating heating)
        {
            if (ModelState.IsValid)
            {
                _context.Add(heating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(heating);
        }

        // GET: Heatings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var heating = await _context.Heatings.FindAsync(id);
            if (heating == null)
            {
                return NotFound();
            }
            return View(heating);
        }

        // POST: Heatings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Heating heating)
        {
            if (id != heating.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(heating);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HeatingExists(heating.Id))
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
            return View(heating);
        }

        // GET: Heatings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var heating = await _context.Heatings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (heating == null)
            {
                return NotFound();
            }

            return View(heating);
        }

        // POST: Heatings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var heating = await _context.Heatings.FindAsync(id);
            if (heating != null)
            {
                _context.Heatings.Remove(heating);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HeatingExists(int id)
        {
            return _context.Heatings.Any(e => e.Id == id);
        }
    }
}
