using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Patient_Tracking_System.Models;
using PostgreSQL.Data;

namespace Patient_Tracking_System.Controllers
{
    public class VisitsController : Controller
    {
        private readonly AppDbContext _context;

        public VisitsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Visits
        public async Task<IActionResult> Index()
        {
             var appDbContext = _context.Visits.Include(v => v.Patient);
             return View(await appDbContext.ToListAsync());
        }

        // GET: Visits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Visits == null)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .Include(v => v.Patient)
                .FirstOrDefaultAsync(m => m.VisitId == id);
            if (visit == null)
            {
                return NotFound();
            }

            return View(visit);
        }

        // GET: Visits/Create
        public IActionResult Create()
        {
            ViewData["PatientID"] = new SelectList(_context.Patients, "Id", "Id");
            return View();
        }

        // POST: Visits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientId,VisitDate,DoctorName,Complaint,TreatmentModalities")] Visit visit)
        {
           if (!ModelState.IsValid)
            {
                _context.Database.ExecuteSqlRaw("CALL sp_addnewvisit({0}, {1}, {2}, {3}, {4})", visit.PatientId, visit.VisitDate, visit.DoctorName, visit.Complaint, visit.TreatmentModalities);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", visit.PatientId);
            return View(visit);
        }

        // GET: Visits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Visits == null)
            {
                return NotFound();
            }

                  var visit = await _context.Visits
              .Include(v => v.Patient)
              .FirstOrDefaultAsync(m => m.VisitId == id);

            if (visit == null)
            {
                return NotFound();
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", visit.PatientId);
            return View(visit);
        }

        // POST: Visits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VisitId,PatientId,VisitDate,DoctorName,Complaint,TreatmentModalities")] Visit visit)
        {
            if (id != visit.VisitId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {       
                    _context.Database.ExecuteSqlRaw("CALL sp_updatevisit({0}, {1}, {2}, {3}, {4}, {5})", visit.VisitId, visit.PatientId, visit.VisitDate, visit.DoctorName, visit.Complaint, visit.TreatmentModalities);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisitExists(visit.VisitId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", visit.PatientId);
            return View(visit);
        }

        // GET: Visits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Visits == null)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .Include(v => v.Patient)
                .FirstOrDefaultAsync(m => m.VisitId == id);
            if (visit == null)
            {
                return NotFound();
            }

            return View(visit);
        }

        // POST: Visits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Visits == null)
            {
                return Problem("Entity set 'AppDbContext.Visits'  is null.");
            }
            var visit = await _context.Visits.FindAsync(id);
            if (visit != null)
            {
               _context.Database.ExecuteSqlRaw("CALL sp_deletevisit({0})", visit.VisitId);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool VisitExists(int id)
        {
          return (_context.Visits?.Any(e => e.PatientId == id)).GetValueOrDefault();
        }
    }
}
