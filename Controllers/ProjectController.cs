using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Controllers
{
    public class ProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
