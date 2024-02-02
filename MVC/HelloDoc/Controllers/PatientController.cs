using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Controllers
{
    public class PatientController : Controller
    {
        [Route("/")]
        public IActionResult PatientSite()
        {
            return View();
        }

        public IActionResult LoginPage()
        {
            return View();
        }

        public IActionResult SubmitRequest()
        {
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        public IActionResult PatientRequest()
        {
            return View();
        }
        public IActionResult FamilyFriendRequest()
        {
            return View();
        }

        public IActionResult ConciergeRequest()
        {
            return View();
        }

        public IActionResult BusinessRequest()
        {
            return View();
        }
    }
}
