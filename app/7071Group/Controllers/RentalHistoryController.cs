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
    public class RentalHistoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RentalHistoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RentalHistory
        public async Task<IActionResult> Index()
        {
            return View(await _context.RentalHistories.ToListAsync());
        }

        // GET: RentalHistory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalHistory = await _context.RentalHistories
                .FirstOrDefaultAsync(m => m.AssetID == id);
            if (rentalHistory == null)
            {
                return NotFound();
            }

            return View(rentalHistory);
        }

        // GET: RentalHistory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RentalHistory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HistoryID,RenterID,AssetID,StartDate,EndDate,RentAmount")] RentalHistory rentalHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rentalHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rentalHistory);
        }

        // GET: RentalHistory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalHistory = await _context.RentalHistories.FindAsync(id);
            if (rentalHistory == null)
            {
                return NotFound();
            }
            return View(rentalHistory);
        }

        // POST: RentalHistory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HistoryID,RenterID,AssetID,StartDate,EndDate,RentAmount")] RentalHistory rentalHistory)
        {
            if (id != rentalHistory.AssetID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rentalHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalHistoryExists(rentalHistory.AssetID))
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
            return View(rentalHistory);
        }

        // GET: RentalHistory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalHistory = await _context.RentalHistories
                .FirstOrDefaultAsync(m => m.AssetID == id);
            if (rentalHistory == null)
            {
                return NotFound();
            }

            return View(rentalHistory);
        }

        // POST: RentalHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rentalHistory = await _context.RentalHistories.FindAsync(id);
            if (rentalHistory != null)
            {
                _context.RentalHistories.Remove(rentalHistory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalHistoryExists(int id)
        {
            return _context.RentalHistories.Any(e => e.AssetID == id);
        }
    }
}
