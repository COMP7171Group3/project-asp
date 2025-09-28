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
    public class DamageReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DamageReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DamageReport
        public async Task<IActionResult> Index()
        {
            return View(await _context.DamageReports.ToListAsync());
        }

        // GET: DamageReport/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var damageReport = await _context.DamageReports
                .FirstOrDefaultAsync(m => m.ReportID == id);
            if (damageReport == null)
            {
                return NotFound();
            }

            return View(damageReport);
        }

        // GET: DamageReport/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DamageReport/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportID,AssetID,Description,RepairCost,ReportDate")] DamageReport damageReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(damageReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(damageReport);
        }

        // GET: DamageReport/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var damageReport = await _context.DamageReports.FindAsync(id);
            if (damageReport == null)
            {
                return NotFound();
            }
            return View(damageReport);
        }

        // POST: DamageReport/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReportID,AssetID,Description,RepairCost,ReportDate")] DamageReport damageReport)
        {
            if (id != damageReport.ReportID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(damageReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DamageReportExists(damageReport.ReportID))
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
            return View(damageReport);
        }

        // GET: DamageReport/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var damageReport = await _context.DamageReports
                .FirstOrDefaultAsync(m => m.ReportID == id);
            if (damageReport == null)
            {
                return NotFound();
            }

            return View(damageReport);
        }

        // POST: DamageReport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var damageReport = await _context.DamageReports.FindAsync(id);
            if (damageReport != null)
            {
                _context.DamageReports.Remove(damageReport);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DamageReportExists(int id)
        {
            return _context.DamageReports.Any(e => e.ReportID == id);
        }
    }
}
