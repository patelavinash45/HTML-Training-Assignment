using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Controllers
{
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public PatientController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

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

        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginPage(AspNetUser user)
        {
            var userFromDb = _dbContext.AspNetUsers.FirstOrDefault(a => a.Email == user.Email);
            if (userFromDb != null && userFromDb.PasswordHash == user.PasswordHash)
            {
                return View(userFromDb);
            }
            else
            {
                return View(null);
            }
        }
    }
}
