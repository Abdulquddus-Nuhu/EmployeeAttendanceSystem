using EAS.Data;
using EAS.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EAS.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public EmployeeController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            return View(await dbContext.Employees.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var employeeExist = dbContext.Employees.FirstOrDefault(x => x.StaffId == employee.StaffId);
                if (employeeExist != null)
                {
                    return RedirectToAction(nameof(ErrorDuplicateEmployee)); 
                }

                dbContext.Employees.Add(employee);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }

            var employee = dbContext.Employees.Find(id);
            if (employee == null)
                return NotFound();

            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                dbContext.Employees.Update(employee);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(employee);
        }
        
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }

            var employee = dbContext.Employees.Find(id);
            if (employee == null)
                return NotFound();

            return View(employee);
        }

        public async Task<IActionResult> ErrorDuplicateEmployee()
        {
            return View();
        }
        

    }
}
