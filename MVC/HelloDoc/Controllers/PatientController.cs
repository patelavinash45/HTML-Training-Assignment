using HelloDoc.DataContext;
using HelloDoc.DataModels;
using HelloDoc.ViewModels;
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
        public IActionResult LoginPage(PatientLogin user)
        {
            var userFromDb = _dbContext.AspNetUsers.FirstOrDefault(a => a.Email == user.Email);
            if (userFromDb == null)
            {
                ViewBag.error = "Email Not Found";
                return View(null);
            }
            else if(userFromDb.PasswordHash != user.PasswordHash)
            {
                ViewBag.error = "Invalid Password";
                return View(null);
            }
            else
            { 
                return RedirectToAction("Dashboard");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PatientRequest(User user)
        {
            AspNetUser aspNetUser = new()
            {
                UserName = user.FirstName,
                Email = user.Email,
                PhoneNumber = user.Mobile,
                PasswordHash = "123456",
                Ip = "123.123.123.123",
                CreatedDate = DateTime.Now
            };
            _dbContext.Add(aspNetUser);
            await _dbContext.SaveChangesAsync();
            aspNetUser= _dbContext.AspNetUsers.FirstOrDefault(a=> a.Email == user.Email);
            user.AspNetUserId = aspNetUser.Id;
            user.CreatedBy = aspNetUser.Id; 
            user.CreatedDate = DateTime.Now;
            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync();
            return View(null);
        }
    }
}
