using EAS.Data;
using EAS.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EAS.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public AttendanceController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
       
        public async Task<IActionResult> Create(Attendance attendance)
        {
            var listOfEmpl = await _dbContext.Employees.ToListAsync();
            ViewData["Employees"] = new SelectList(listOfEmpl, "Id", "Name");

            if (ModelState.IsValid)
            {
                await _dbContext.Attendances.AddAsync(attendance);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(attendance);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Attendance> attendanceList = await _dbContext.Attendances.ToListAsync();
            return View(attendanceList);
        }
    }
}
