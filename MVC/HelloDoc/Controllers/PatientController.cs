using HelloDoc.DataContext;
using HelloDoc.DataModels;
using HelloDoc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
        [ValidateAntiForgeryToken]
        public IActionResult LoginPage(PatientLogin user)
        {
            if(ModelState.IsValid)
            {
                var userFromDb = _dbContext.AspNetUsers.FirstOrDefault(a => a.Email.Trim() == user.Email.Trim());
                if (userFromDb == null)
                {
                    ViewBag.error = 1;
                    return View(null);
                }
                else if (userFromDb.PasswordHash != user.PasswordHash)
                {
                    ViewBag.error = 2;
                    return View(null);
                }
                else
                {
                    return RedirectToAction("Dashboard");
                }
            }
            else
            {
                return View(null);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PatientRequest(AddPatientRequest model)
        {
            if (ModelState.IsValid)
            {
                AspNetRole aspNetRole = _dbContext.AspNetRoles.FirstOrDefault(a => a.Name.Trim() == "Patient");
                if(aspNetRole == null)
                {
                    aspNetRole = new()
                    {
                        Name = "Patient",
                    };
                    _dbContext.Add(aspNetRole);
                    await _dbContext.SaveChangesAsync();
                    aspNetRole = _dbContext.AspNetRoles.FirstOrDefault(a => a.Name.Trim() == "Patient");
                };
                //
                AspNetUser aspNetUser = new()
                {
                    UserName = model.FirstName,
                    Email = model.Email,
                    PhoneNumber = model.Mobile,
                    PasswordHash = "123456",
                    Ip = "123.123.123.123",
                    CreatedDate = DateTime.Now
                };
                _dbContext.Add(aspNetUser);
                await _dbContext.SaveChangesAsync();
                //
                aspNetUser = _dbContext.AspNetUsers.FirstOrDefault(a => a.Email.Trim() == model.Email.Trim());
                User user = new()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Mobile = model.Mobile,  
                    Street = model.Street,
                    City = model.City,
                    State = model.State,
                    ZipCode = model.ZipCode,
                    AspNetUserId = aspNetUser.Id,
                    CreatedBy = aspNetUser.Id,
                    CreatedDate = DateTime.Now,
                    House=model.House,
                    //IntYear=model.BirthDate.Value.Year,
                    //IntDate= int.Parse(model.BirthDate.Value.Date.ToString()),
                    //StrMonth= model.BirthDate.Value.Month.ToString(),
                };
                _dbContext.Add(user);
                await _dbContext.SaveChangesAsync();
                //
                AspNetUserRole aspNetUserRole = new()
                {
                    UserId = user.UserId,
                    RoleId = aspNetRole.Id,
                };
                _dbContext.Add(aspNetUserRole);
                await _dbContext.SaveChangesAsync();
                //
                user = _dbContext.Users.FirstOrDefault(a => a.Email.Trim() == user.Email.Trim());
                Request request = new()
                {
                    RequestTypeId = 2,
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.Mobile,
                    CreatedDate = DateTime.Now,
                };
                _dbContext.Add(request);
                await _dbContext.SaveChangesAsync();
                //
                Region region = _dbContext.Regions.FirstOrDefault(a => a.Name.Trim() == model.State.Trim());
                if (region == null)
                {
                    region = new()
                    {
                        Name = model.State,
                    };
                    _dbContext.Add(region);
                    await _dbContext.SaveChangesAsync();
                    region = _dbContext.Regions.FirstOrDefault(a => a.Name.Trim() == model.State.Trim());
                }
                //
                request = _dbContext.Requests.FirstOrDefault(a => a.Email.Trim() == model.Email.Trim());
                RequestClient requestClient = new()
                {
                    RequestId = request.RequestId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.Mobile,
                    RegionId = region.RegionId,
                    Email = model.Email,
                    State = model.State,
                    Street = model.Street,
                    City = model.City,
                    ZipCode = model.ZipCode,
                    Status = 1,
                    Symptoms = model.Symptoms,
                }; 
                _dbContext.Add(requestClient);
                await _dbContext.SaveChangesAsync();
            };
            return View(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConciergeRequest(AddConciergeRequest model)
        {
            if (ModelState.IsValid)
            {
                AspNetRole aspNetRole = _dbContext.AspNetRoles.FirstOrDefault(a => a.Name.Trim() == "Patient");
                if (aspNetRole == null)
                {
                    aspNetRole = new()
                    {
                        Name = "Patient",
                    };
                    _dbContext.Add(aspNetRole);
                    await _dbContext.SaveChangesAsync();
                    aspNetRole = _dbContext.AspNetRoles.FirstOrDefault(a => a.Name.Trim() == "Patient");
                };
                //
                AspNetUser aspNetUser = new()
                {
                    UserName = model.FirstName,
                    Email = model.Email,
                    PhoneNumber = model.Mobile,
                    PasswordHash = "123456",
                    Ip = "123.123.123.123",
                    CreatedDate = DateTime.Now
                };
                _dbContext.Add(aspNetUser);
                await _dbContext.SaveChangesAsync();
                //
                aspNetUser = _dbContext.AspNetUsers.FirstOrDefault(a => a.Email.Trim() == model.Email.Trim());
                AspNetUserRole aspNetUserRole = new()
                {
                    UserId = aspNetUser.Id,
                    RoleId = aspNetRole.Id,
                };
                _dbContext.Add(aspNetUserRole);
                await _dbContext.SaveChangesAsync();
                //
                User user = new()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Mobile = model.Mobile,
                    Street = model.Street,
                    City = model.City,
                    State = model.State,
                    ZipCode = model.ZipCode,
                    AspNetUserId = aspNetUser.Id,
                    CreatedBy = aspNetUser.Id,
                    CreatedDate = DateTime.Now,
                    House = model.House,
                    //IntYear=model.BirthDate.Value.Year,
                    //IntDate= int.Parse(model.BirthDate.Value.Date.ToString()),
                    //StrMonth= model.BirthDate.Value.Month.ToString(),
                };
                _dbContext.Add(user);
                await _dbContext.SaveChangesAsync();
                //
                Request request = new()
                {
                    RequestTypeId = 4,
                    FirstName = model.ConciergeFirstName,
                    LastName = model.ConciergeLastName,
                    Email = model.ConciergeEmail,
                    PhoneNumber = model.ConciergeMobile,
                };
                _dbContext.Add(request);
                await _dbContext.SaveChangesAsync();
                //
                Region region =_dbContext.Regions.FirstOrDefault(a => a.Name.Trim() == model.ConciergeState.Trim());
                if(region == null)
                {
                    region = new()
                    {
                        Name = model.ConciergeState,
                    };
                    _dbContext.Add(region);
                    await _dbContext.SaveChangesAsync();
                    region = _dbContext.Regions.FirstOrDefault(a => a.Name.Trim() == model.ConciergeState.Trim());
                }
                //
                Concierge concierge = new()
                {
                    ConciergeName = model.ConciergeFirstName ,
                    Street = model.ConciergeStreet,
                    City = model.ConciergeCity,
                    State = model.ConciergeState,
                    ZipCode = model.ConciergeZipCode,
                    CreatedDate = DateTime.Now,
                    RegionId=region.RegionId,
                };
                _dbContext.Add(concierge);  
                await _dbContext.SaveChangesAsync();
                //
                concierge = _dbContext.Concierges.FirstOrDefault(a => a.ConciergeName.Trim() == model.ConciergeFirstName.Trim());
                request = _dbContext.Requests.FirstOrDefault(a => a.Email.Trim() == model.ConciergeEmail.Trim());
                RequestConcierge requestConcierge = new()
                {
                    RequestId = request.RequestId,
                    ConciergeId = concierge.ConciergeId
                };  
                _dbContext.Add(requestConcierge);
                await _dbContext.SaveChangesAsync();
                //
                region = _dbContext.Regions.FirstOrDefault(a => a.Name.Trim() == model.State.Trim());
                if (region == null)
                {
                    region = new()
                    {
                        Name = model.State,
                    };
                    _dbContext.Add(region);
                    await _dbContext.SaveChangesAsync();
                    region = _dbContext.Regions.FirstOrDefault(a => a.Name.Trim() == model.State.Trim());
                }
                //
                request = _dbContext.Requests.FirstOrDefault(a => a.Email.Trim() == model.ConciergeEmail.Trim());
                RequestClient requestClient = new()
                {
                    RequestId = request.RequestId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.Mobile,
                    RegionId = region.RegionId,
                    Email = model.Email,
                    State = model.State,
                    Street = model.Street,
                    City = model.City,
                    ZipCode = model.ZipCode,
                    Status = 1,
                    Symptoms = model.Symptoms,
                };
                _dbContext.Add(requestClient);
                await _dbContext.SaveChangesAsync();
            };
            return View(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FamilyFriendRequest(AddFamilyRequest model)
        {
            if (ModelState.IsValid)
            {
                AspNetRole aspNetRole = _dbContext.AspNetRoles.FirstOrDefault(a => a.Name.Trim() == "Patient");
                if (aspNetRole == null)
                {
                    aspNetRole = new()
                    {
                        Name = "Patient",
                    };
                    _dbContext.Add(aspNetRole);
                    await _dbContext.SaveChangesAsync();
                    aspNetRole = _dbContext.AspNetRoles.FirstOrDefault(a => a.Name.Trim() == "Patient");
                };
                //
                AspNetUser aspNetUser = new()
                {
                    UserName = model.FirstName,
                    Email = model.Email,
                    PhoneNumber = model.Mobile,
                    PasswordHash = "123456",
                    Ip = "123.123.123.123",
                    CreatedDate = DateTime.Now
                };
                _dbContext.Add(aspNetUser);
                await _dbContext.SaveChangesAsync();
                //
                aspNetUser = _dbContext.AspNetUsers.FirstOrDefault(a => a.Email.Trim() == model.Email.Trim());
                AspNetUserRole aspNetUserRole = new()
                {
                    UserId = aspNetUser.Id,
                    RoleId = aspNetRole.Id,
                };
                _dbContext.Add(aspNetUserRole);
                await _dbContext.SaveChangesAsync();
                //
                User user = new()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Mobile = model.Mobile,
                    Street = model.Street,
                    City = model.City,
                    State = model.State,
                    ZipCode = model.ZipCode,
                    AspNetUserId = aspNetUser.Id,
                    CreatedBy = aspNetUser.Id,
                    CreatedDate = DateTime.Now,
                    House = model.House,
                    //IntYear=model.BirthDate.Value.Year,
                    //IntDate= int.Parse(model.BirthDate.Value.Date.ToString()),
                    //StrMonth= model.BirthDate.Value.Month.ToString(),
                };
                _dbContext.Add(user);
                await _dbContext.SaveChangesAsync();
                //
                Request request = new()
                {
                    RequestTypeId = 4,
                    FirstName = model.FamilyFriendFirstName,
                    LastName = model.FamilyFriendLastName,
                    Email = model.FamilyFriendEmail,
                    PhoneNumber = model.FamilyFriendMobile,
                };
                _dbContext.Add(request);
                await _dbContext.SaveChangesAsync();
                //
                Region region = _dbContext.Regions.FirstOrDefault(a => a.Name.Trim() == model.State.Trim());
                if (region == null)
                {
                    region = new()
                    {
                        Name = model.State,
                    };
                    _dbContext.Add(region);
                    await _dbContext.SaveChangesAsync();
                    region = _dbContext.Regions.FirstOrDefault(a => a.Name.Trim() == model.State.Trim());
                }
                //
                request = _dbContext.Requests.FirstOrDefault(a => a.Email.Trim() == model.FamilyFriendEmail.Trim());
                RequestClient requestClient = new()
                {
                    RequestId = request.RequestId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.Mobile,
                    RegionId = region.RegionId,
                    Email = model.Email,
                    State = model.State,
                    Street = model.Street,
                    City = model.City,
                    ZipCode = model.ZipCode,
                    Status = 1,
                    Symptoms = model.Symptoms,
                };
                _dbContext.Add(requestClient);
                await _dbContext.SaveChangesAsync();
            };
            return View(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BusinessRequest(AddBusinessRequest model)
        {
            if (ModelState.IsValid)
            {
                AspNetRole aspNetRole = _dbContext.AspNetRoles.FirstOrDefault(a => a.Name.Trim() == "Patient");
                if (aspNetRole == null)
                {
                    aspNetRole = new()
                    {
                        Name = "Patient",
                    };
                    _dbContext.Add(aspNetRole);
                    await _dbContext.SaveChangesAsync();
                    aspNetRole = _dbContext.AspNetRoles.FirstOrDefault(a => a.Name.Trim() == "Patient");
                };
                //
                AspNetUser aspNetUser = new()
                {
                    UserName = model.FirstName,
                    Email = model.Email,
                    PhoneNumber = model.Mobile,
                    PasswordHash = "123456",
                    Ip = "123.123.123.123",
                    CreatedDate = DateTime.Now
                };
                _dbContext.Add(aspNetUser);
                await _dbContext.SaveChangesAsync();
                //
                aspNetUser = _dbContext.AspNetUsers.FirstOrDefault(a => a.Email.Trim() == model.Email.Trim());
                AspNetUserRole aspNetUserRole = new()
                {
                    UserId = aspNetUser.Id,
                    RoleId = aspNetRole.Id,
                };
                _dbContext.Add(aspNetUserRole);
                await _dbContext.SaveChangesAsync();
                //
                User user = new()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Mobile = model.Mobile,
                    Street = model.Street,
                    City = model.City,
                    State = model.State,
                    ZipCode = model.ZipCode,
                    AspNetUserId = aspNetUser.Id,
                    CreatedBy = aspNetUser.Id,
                    CreatedDate = DateTime.Now,
                    House = model.House,
                    //IntYear=model.BirthDate.Value.Year,
                    //IntDate= int.Parse(model.BirthDate.Value.Date.ToString()),
                    //StrMonth= model.BirthDate.Value.Month.ToString(),
                };
                _dbContext.Add(user);
                await _dbContext.SaveChangesAsync();
                //
                Request request = new()
                {
                    RequestTypeId = 4,
                    FirstName = model.BusinessFirstName,
                    LastName = model.BusinessLastName,
                    Email = model.BusinessEmail,
                    PhoneNumber = model.BusinessMobile,
                };
                _dbContext.Add(request);
                await _dbContext.SaveChangesAsync();
                //
                Business business = new()
                {
                    Name = model.BusinessFirstName,
                    PhoneNumber= model.BusinessMobile,
                    CreatedDate = DateTime.Now,
                };
                _dbContext.Add(business);
                await _dbContext.SaveChangesAsync();
                //
                business = _dbContext.Businesses.FirstOrDefault(a => a.Name.Trim() == model.BusinessFirstName.Trim());
                request = _dbContext.Requests.FirstOrDefault(a => a.Email.Trim() == model.BusinessEmail.Trim());
                RequestBusiness requestBusiness = new()
                {
                    RequestId = request.RequestId,
                    BusinessId = business.BusinessId,
                };
                _dbContext.Add(requestBusiness);
                await _dbContext.SaveChangesAsync();
                //
                Region region = _dbContext.Regions.FirstOrDefault(a => a.Name.Trim() == model.State.Trim());
                if (region == null)
                {
                    region = new()
                    {
                        Name = model.State,
                    };
                    _dbContext.Add(region);
                    await _dbContext.SaveChangesAsync();
                    region = _dbContext.Regions.FirstOrDefault(a => a.Name.Trim() == model.State.Trim());
                }
                //
                request = _dbContext.Requests.FirstOrDefault(a => a.Email.Trim() == model.BusinessEmail.Trim());
                RequestClient requestClient = new()
                {
                    RequestId = request.RequestId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.Mobile,
                    RegionId = region.RegionId,
                    Email = model.Email,
                    State = model.State,
                    Street = model.Street,
                    City = model.City,
                    ZipCode = model.ZipCode,
                    Status = 1,
                    Symptoms = model.Symptoms,
                };
                _dbContext.Add(requestClient);
                await _dbContext.SaveChangesAsync();
            };
            return View(null);
        }
    }
}
