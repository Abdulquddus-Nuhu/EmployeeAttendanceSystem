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
        public async Task<IActionResult> Edit(int? id)
        {
            var employeeToUpdate = await dbContext.Employees.FirstOrDefaultAsync(s => s.Id == id);
            if (await TryUpdateModelAsync<Employee>( employeeToUpdate,"", e => e.Name, e => e.Gender))
            {
                try
                {
                    await dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(employeeToUpdate);
        }
        public async Task<IActionResult> Delete()
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
    }
}
