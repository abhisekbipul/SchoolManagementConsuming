using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementConsuming.Controllers
{
    public class AcademicCalendarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
