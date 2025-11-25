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

    public class BuildingDirectionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BuildingDirectionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BuildingDirections
        public async Task<IActionResult> Index()
        {
            return View(await _context.BuildingDirections.ToListAsync());
        }

        // GET: BuildingDirections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buildingDirection = await _context.BuildingDirections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (buildingDirection == null)
            {
                return NotFound();
            }

            return View(buildingDirection);
        }

        // GET: BuildingDirections/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BuildingDirections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] BuildingDirection buildingDirection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(buildingDirection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(buildingDirection);
        }

        // GET: BuildingDirections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buildingDirection = await _context.BuildingDirections.FindAsync(id);
            if (buildingDirection == null)
            {
                return NotFound();
            }
            return View(buildingDirection);
        }

        // POST: BuildingDirections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] BuildingDirection buildingDirection)
        {
            if (id != buildingDirection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(buildingDirection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuildingDirectionExists(buildingDirection.Id))
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
            return View(buildingDirection);
        }

        // GET: BuildingDirections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buildingDirection = await _context.BuildingDirections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (buildingDirection == null)
            {
                return NotFound();
            }

            return View(buildingDirection);
        }

        // POST: BuildingDirections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var buildingDirection = await _context.BuildingDirections.FindAsync(id);
            if (buildingDirection != null)
            {
                _context.BuildingDirections.Remove(buildingDirection);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuildingDirectionExists(int id)
        {
            return _context.BuildingDirections.Any(e => e.Id == id);
        }
    }
}
