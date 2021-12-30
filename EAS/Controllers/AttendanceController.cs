using Microsoft.AspNetCore.Mvc;

namespace EAS.Controllers
{
    public class AttendanceController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
    }
}
