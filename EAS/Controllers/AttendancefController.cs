using EAS.Data;
using EAS.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EAS.Controllers
{
    public class AttendancefController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public AttendancefController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Attendance> attendanceList = await _dbContext.Attendances.Include(e => e.Employee).ToListAsync();
            return View(attendanceList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Employees"] = new SelectList(_dbContext.Employees, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(attendance);
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(attendance);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }

            var attendance = _dbContext.Attendances.Include(e=>e.Employee).FirstOrDefault(a=>a.EmployeeId == id);

            if (attendance == null)
                return NotFound();

            return View(attendance);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Attendances.Update(attendance);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(attendance);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }

            var employeeAttendance = _dbContext.Attendances.Include(e=>e.Employee).FirstOrDefault(e => e.Id == id);

            if (employeeAttendance == null)
                return NotFound();

            return View(employeeAttendance);
        }
    }
}
