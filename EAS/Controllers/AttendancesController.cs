#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EAS.Data;
using EAS.Data.Entities;

namespace EAS.Controllers
{
    public class AttendancesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Attendances
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Attendances.Include(a => a.Employee);
            /* .Where(a=>a.Date.Date == DateTime.Today); */
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Attendances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // GET: Attendances/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name");
            return View();
        }

        // POST: Attendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Mode,EmployeeId")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                var attendanceExist = _context.Attendances.Include(e=>e.Employee).Where(d => d.Date.Date == DateTime.Today);

                if (attendanceExist.Any( e=>e.EmployeeId == attendance.EmployeeId))
                    return RedirectToAction(nameof(ErrorDuplicateAttendance));

                if (attendance.Date.Date < DateTime.Today)
                    return RedirectToAction(nameof(ErrorExpiredAttendance));


                _context.Add(attendance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name", attendance.EmployeeId);
            return View(attendance);
        }

        // GET: Attendances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", attendance.EmployeeId);
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Mode,EmployeeId")] Attendance attendance)
        {

            if (id != attendance.Id)
            {
                return NotFound();
            }

            var attendanceListToday = _context.Attendances.Where(e => e.Date.Date == DateTime.Today);
            if (attendanceListToday.Any(e => e.EmployeeId == attendance.EmployeeId))
                return RedirectToAction(nameof(ErrorDuplicateAttendance));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceExists(attendance.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", attendance.EmployeeId);
            return View(attendance);
        }


        private bool AttendanceExists(int id)
        {
            return _context.Attendances.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ErrorDuplicateAttendance()
        {
            return View();
        }
        
        public async Task<IActionResult> ErrorExpiredAttendance()
        {
            return View();
        }
        
    }
}
