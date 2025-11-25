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

    public class FloorMaterialsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FloorMaterialsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FloorMaterials
        public async Task<IActionResult> Index()
        {
            return View(await _context.floorMaterials.ToListAsync());
        }

        // GET: FloorMaterials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var floorMaterial = await _context.floorMaterials
                .FirstOrDefaultAsync(m => m.Id == id);
            if (floorMaterial == null)
            {
                return NotFound();
            }

            return View(floorMaterial);
        }

        // GET: FloorMaterials/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FloorMaterials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] FloorMaterial floorMaterial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(floorMaterial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(floorMaterial);
        }

        // GET: FloorMaterials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var floorMaterial = await _context.floorMaterials.FindAsync(id);
            if (floorMaterial == null)
            {
                return NotFound();
            }
            return View(floorMaterial);
        }

        // POST: FloorMaterials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] FloorMaterial floorMaterial)
        {
            if (id != floorMaterial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(floorMaterial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FloorMaterialExists(floorMaterial.Id))
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
            return View(floorMaterial);
        }

        // GET: FloorMaterials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var floorMaterial = await _context.floorMaterials
                .FirstOrDefaultAsync(m => m.Id == id);
            if (floorMaterial == null)
            {
                return NotFound();
            }

            return View(floorMaterial);
        }

        // POST: FloorMaterials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var floorMaterial = await _context.floorMaterials.FindAsync(id);
            if (floorMaterial != null)
            {
                _context.floorMaterials.Remove(floorMaterial);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FloorMaterialExists(int id)
        {
            return _context.floorMaterials.Any(e => e.Id == id);
        }
    }
}
