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
    public class ServiceRegistrationController : Controller
    {
        private readonly CareDbContext _context;

        public ServiceRegistrationController(CareDbContext context)
        {
            _context = context;
        }

        // GET: ServiceRegistration
        public async Task<IActionResult> Index()
        {
            return View(await _context.ServiceRegistrations.ToListAsync());
        }

        // GET: ServiceRegistration/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRegistration = await _context.ServiceRegistrations
                .FirstOrDefaultAsync(m => m.ClientID == id);
            if (serviceRegistration == null)
            {
                return NotFound();
            }

            return View(serviceRegistration);
        }

        // GET: ServiceRegistration/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceRegistration/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RegistrationID,ClientID,ServiceID,RegistrationDate")] ServiceRegistration serviceRegistration)
        {
            if (ModelState.IsValid)
            {
                using var tx = await _context.Database.BeginTransactionAsync();
                try
                {
                    _context.ServiceRegistrations.Add(serviceRegistration);
                    await _context.SaveChangesAsync();
                    await tx.CommitAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            return View(serviceRegistration);
        }

        // GET: ServiceRegistration/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRegistration = await _context.ServiceRegistrations.FindAsync(id);
            if (serviceRegistration == null)
            {
                return NotFound();
            }
            return View(serviceRegistration);
        }

        // POST: ServiceRegistration/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RegistrationID,ClientID,ServiceID,RegistrationDate")] ServiceRegistration serviceRegistration)
        {
            if (id != serviceRegistration.ClientID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using var tx = await _context.Database.BeginTransactionAsync();
                try
                {
                    _context.ServiceRegistrations.Update(serviceRegistration);
                    await _context.SaveChangesAsync();
                    await tx.CommitAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    await tx.RollbackAsync();

                    if (!ServiceRegistrationExists(serviceRegistration.ServiceID))
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
            return View(serviceRegistration);
        }

        // GET: ServiceRegistration/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRegistration = await _context.ServiceRegistrations
                .FirstOrDefaultAsync(m => m.ClientID == id);
            if (serviceRegistration == null)
            {
                return NotFound();
            }

            return View(serviceRegistration);
        }

        // POST: ServiceRegistration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceRegistration = await _context.ServiceRegistrations.FindAsync(id);
            if (serviceRegistration != null)
            {
                using var tx = await _context.Database.BeginTransactionAsync();
                try
                {
                    _context.ServiceRegistrations.Remove(serviceRegistration);
                    await _context.SaveChangesAsync();
                    await tx.CommitAsync();
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceRegistrationExists(int id)
        {
            return _context.ServiceRegistrations.Any(e => e.ClientID == id);
        }
    }
}
