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
    public class HotWaterSuppliersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HotWaterSuppliersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HotWaterSuppliers
        public async Task<IActionResult> Index()
        {
            return View(await _context.HotWaterSuppliers.ToListAsync());
        }

        // GET: HotWaterSuppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotWaterSupplier = await _context.HotWaterSuppliers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotWaterSupplier == null)
            {
                return NotFound();
            }

            return View(hotWaterSupplier);
        }

        // GET: HotWaterSuppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HotWaterSuppliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] HotWaterSupplier hotWaterSupplier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hotWaterSupplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hotWaterSupplier);
        }

        // GET: HotWaterSuppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotWaterSupplier = await _context.HotWaterSuppliers.FindAsync(id);
            if (hotWaterSupplier == null)
            {
                return NotFound();
            }
            return View(hotWaterSupplier);
        }

        // POST: HotWaterSuppliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] HotWaterSupplier hotWaterSupplier)
        {
            if (id != hotWaterSupplier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hotWaterSupplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotWaterSupplierExists(hotWaterSupplier.Id))
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
            return View(hotWaterSupplier);
        }

        // GET: HotWaterSuppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotWaterSupplier = await _context.HotWaterSuppliers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotWaterSupplier == null)
            {
                return NotFound();
            }

            return View(hotWaterSupplier);
        }

        // POST: HotWaterSuppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hotWaterSupplier = await _context.HotWaterSuppliers.FindAsync(id);
            if (hotWaterSupplier != null)
            {
                _context.HotWaterSuppliers.Remove(hotWaterSupplier);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotWaterSupplierExists(int id)
        {
            return _context.HotWaterSuppliers.Any(e => e.Id == id);
        }
    }
}
