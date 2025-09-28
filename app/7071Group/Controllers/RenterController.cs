using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _7071Group.Data;
using _7071Group.Models;

namespace _7071Group.Controllers
{
    public class RenterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RenterController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Renter
        public async Task<IActionResult> Index()
        {
            return View(await _context.Renters.ToListAsync());
        }

        // GET: Renter/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var renter = await _context.Renters
                .FirstOrDefaultAsync(m => m.RenterID == id);
            if (renter == null)
            {
                return NotFound();
            }

            return View(renter);
        }

        // GET: Renter/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Renter/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RenterID,Name,EmergencyContact,FamilyDoctor")] Renter renter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(renter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(renter);
        }

        // GET: Renter/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var renter = await _context.Renters.FindAsync(id);
            if (renter == null)
            {
                return NotFound();
            }
            return View(renter);
        }

        // POST: Renter/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RenterID,Name,EmergencyContact,FamilyDoctor")] Renter renter)
        {
            if (id != renter.RenterID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(renter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RenterExists(renter.RenterID))
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
            return View(renter);
        }

        // GET: Renter/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var renter = await _context.Renters
                .FirstOrDefaultAsync(m => m.RenterID == id);
            if (renter == null)
            {
                return NotFound();
            }

            return View(renter);
        }

        // POST: Renter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var renter = await _context.Renters.FindAsync(id);
            if (renter != null)
            {
                _context.Renters.Remove(renter);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RenterExists(int id)
        {
            return _context.Renters.Any(e => e.RenterID == id);
        }
    }
}
