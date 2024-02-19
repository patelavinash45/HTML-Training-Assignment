using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Controllers
{
    public class Admin : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
