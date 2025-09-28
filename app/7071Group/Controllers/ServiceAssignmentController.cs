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
    public class ServiceAssignmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceAssignmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ServiceAssignment
        public async Task<IActionResult> Index()
        {
            return View(await _context.ServiceAssignments.ToListAsync());
        }

        // GET: ServiceAssignment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceAssignment = await _context.ServiceAssignments
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (serviceAssignment == null)
            {
                return NotFound();
            }

            return View(serviceAssignment);
        }

        // GET: ServiceAssignment/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceAssignment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AssignedID,EmployeeID,ServiceID,ScheduledDate")] ServiceAssignment serviceAssignment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviceAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serviceAssignment);
        }

        // GET: ServiceAssignment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceAssignment = await _context.ServiceAssignments.FindAsync(id);
            if (serviceAssignment == null)
            {
                return NotFound();
            }
            return View(serviceAssignment);
        }

        // POST: ServiceAssignment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AssignedID,EmployeeID,ServiceID,ScheduledDate")] ServiceAssignment serviceAssignment)
        {
            if (id != serviceAssignment.EmployeeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceAssignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceAssignmentExists(serviceAssignment.EmployeeID))
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
            return View(serviceAssignment);
        }

        // GET: ServiceAssignment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceAssignment = await _context.ServiceAssignments
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (serviceAssignment == null)
            {
                return NotFound();
            }

            return View(serviceAssignment);
        }

        // POST: ServiceAssignment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceAssignment = await _context.ServiceAssignments.FindAsync(id);
            if (serviceAssignment != null)
            {
                _context.ServiceAssignments.Remove(serviceAssignment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceAssignmentExists(int id)
        {
            return _context.ServiceAssignments.Any(e => e.EmployeeID == id);
        }
    }
}
