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
       
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                dbContext.Employees.Add(employee);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }
        public async Task<IActionResult> Edit(int? id)
        {
            var employeeToUpdate = await dbContext.Employees.FindAsync(id);

            if (employeeToUpdate == null)
            {
                return NotFound();
            }
            return View(employeeToUpdate);
        } 
        public async Task<IActionResult> Delete(int? id)
        {
            var employeeToDelete = await dbContext.Employees.FindAsync(id);

            if (ModelState.IsValid)
            {
                 dbContext.Employees.Remove(employeeToDelete);
                 await dbContext.SaveChangesAsync();
                 return RedirectToAction("Index");
            }          
            return View(employeeToDelete);
        }
       
    }
}
