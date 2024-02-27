using AspNetCoreHero.ToastNotification.Abstractions;
using HelloDoc.DataContext;
using HelloDoc.DataModels;
using HelloDoc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Repositories.ViewModels;
using Services.Interfaces.Patient;

namespace HelloDoc.Controllers
{
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly INotyfService _notyfService;
        private readonly ILoginService _loginService;
        private readonly IPatientDashboardService _dashboardService;
        private readonly IAddRequestService _addRequestService;
        private readonly IViewProfileService _viewProfileService;
        private readonly IResetPasswordService _resetPasswordService;
        private readonly IViewDocumentsServices _viewDocumentsServices;

        public PatientController(ApplicationDbContext dbContext,INotyfService notyfService,ILoginService loginService ,IPatientDashboardService dashboardService,
                                 IAddRequestService addRequestService,IViewProfileService viewProfileService, IResetPasswordService resetPasswordService,
                                 IViewDocumentsServices viewDocumentsServices)
        {
            _dbContext = dbContext;
            _notyfService = notyfService;
            _loginService = loginService;
            _dashboardService = dashboardService;
            _addRequestService = addRequestService;
            _viewProfileService= viewProfileService;
            _resetPasswordService= resetPasswordService;
            _viewDocumentsServices = viewDocumentsServices;
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

        public IActionResult NewPassword(String token, int id, string time)
        {
            return View(_resetPasswordService.validatePasswordLink(token, id, time));
        }

        public IActionResult RequestForSomeOne()
        {
            int aspNetUserId = HttpContext.Session.GetInt32("aspNetUserId").Value;
            return View(_addRequestService.getModelForRequestForSomeoneelse(aspNetUserId));
        }

        public IActionResult RequestForMe()
        {
            int aspNetUserId = HttpContext.Session.GetInt32("aspNetUserId").Value;
            return View(_addRequestService.getModelForRequestByMe(aspNetUserId));
        }

        public IActionResult ViewProfile()
        {
            int aspNetUserId = HttpContext.Session.GetInt32("aspNetUserId").Value;
            return View(_viewProfileService.getProfileDetails(aspNetUserId));
        }

        public IActionResult ViewDocument(int id)
        {
            int aspNetUserId = HttpContext.Session.GetInt32("aspNetUserId").Value;
            return View(_viewDocumentsServices.getDocumentList(requestId: id,aspNetUserId: aspNetUserId));
        }

        public IActionResult Dashboard()
        {
            int aspNetUserId = HttpContext.Session.GetInt32("aspNetUserId").Value;
            return View(_dashboardService.GetUsersMedicalData(aspNetUserId));
        }

        [HttpGet]
        public JsonResult CheckEmailExists(string email)
        {
            var emailExists = _addRequestService.IsEmailExists(email);
            return Json(new { emailExists });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginPage(PatientLogin model)
        {
            if (ModelState.IsValid)
            {
                int aspNetUserId = _loginService.auth(model);
                if (aspNetUserId == 0)
                {
                    _notyfService.Error("Invalid credentials");
                    return View(null);
                }
                else
                {
                    HttpContext.Session.SetInt32("aspNetUserId", aspNetUserId);
                    _notyfService.Success("Successfully Login");
                    return RedirectToAction("Dashboard", "Patient");
                }
            }
            return View(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewPassword(SetNewPassword model)
        {
            if (ModelState.IsValid)
            {
                if(await _resetPasswordService.changePassword(model))
                {
                    _notyfService.Success("Successfully Password Updated");
                    return RedirectToAction("LoginPage", "Patient");
                }
            }
            return View(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewProfile(ViewProfile model)
        {
            if (await _viewProfileService.updatePatientProfile(model))
            {
                _notyfService.Success("Successfully Updated");
            }
            else
            {
                _notyfService.Error("Faild!");
            }
            return RedirectToAction("ViewProfile", "Patient");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            return await _resetPasswordService.resetPasswordLinkSend(model.Email) ? RedirectToAction("PatientSite", "Patient") : View(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewDocument(ViewDocument model)
        {
            if (model.File != null)
            {
                if(await _viewDocumentsServices.uploadFile(model)>0)
                {
                    return RedirectToAction("ViewDocument", "Patient", new { id = model.RequestId });
                }
            }
            return View(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestForMe(AddRequestByPatient model)
        {
            if (ModelState.IsValid)
            {
                if (await _addRequestService.addRequestForMe(model))
                {
                    _notyfService.Success("Successfully Request Added");
                    return RedirectToAction("Dashboard", "Patient");
                }
                else
                {
                    _notyfService.Error("Add Request Faild");
                }
            }
            return View(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestForSomeOne(AddRequestByPatient model)
        {
            if (ModelState.IsValid)
            {
                int aspNetUserId = HttpContext.Session.GetInt32("aspNetUserId").Value;
                if (await _addRequestService.addRequestForSomeOneelse(model : model , aspNetUserIdMe: aspNetUserId))
                {
                    _notyfService.Success("Successfully Request Added");
                    return RedirectToAction("Dashboard", "Patient");
                }
                else
                {
                    _notyfService.Error("Add Request Faild");
                }
            }
            return View(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PatientRequest(AddPatientRequest model)
        {
            if (ModelState.IsValid)
            {
                if (await _addRequestService.addPatientRequest(model))
                {
                    _notyfService.Success("Successfully Request Added");
                    return RedirectToAction("LoginPage", "Patient");
                }
                else
                {
                    _notyfService.Error("Add Request Faild");
                }
            };
            return View(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConciergeRequest(AddConciergeRequest model)
        {
            if (ModelState.IsValid)
            {
                if (await _addRequestService.addConciergeRequest(model))
                {
                    _notyfService.Success("Successfully Request Added");
                    return RedirectToAction("LoginPage", "Patient");
                }
                else
                {
                    _notyfService.Error("Add Request Faild");
                }
            }
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
                    //aspNetRole = _dbContext.AspNetRoles.FirstOrDefault(a => a.Name.Trim() == "Patient");
                };
                //
                AspNetUser aspNetUser = _dbContext.AspNetUsers.FirstOrDefault(a => a.Email.Trim() == model.Email.Trim());
                User user = _dbContext.Users.FirstOrDefault(a => a.Email.Trim() == model.Email.Trim());
                if (aspNetUser == null)
                {
                    aspNetUser = new()
                    {
                        UserName = model.FirstName,
                        Email = model.Email,
                        PhoneNumber = model.Mobile,
                        PasswordHash = model.Password,
                        CreatedDate = DateTime.Now
                    };
                    _dbContext.Add(aspNetUser);
                    await _dbContext.SaveChangesAsync();
                    //
                    //aspNetUser = _dbContext.AspNetUsers.FirstOrDefault(a => a.Email.Trim() == model.Email.Trim());
                    user = new()
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
                        IntYear = model.BirthDate.Value.Year,
                        IntDate = model.BirthDate.Value.Day,
                        StrMonth = model.BirthDate.Value.Month.ToString(),
                    };
                    _dbContext.Add(user);
                    await _dbContext.SaveChangesAsync();
                    //
                    //aspNetUser = _dbContext.AspNetUsers.FirstOrDefault(a => a.Email.Trim() == model.Email.Trim());
                    AspNetUserRole aspNetUserRole = new()
                    {
                        UserId = aspNetUser.Id,
                        RoleId = aspNetRole.Id,
                    };
                    _dbContext.Add(aspNetUserRole);
                    await _dbContext.SaveChangesAsync();
                }
                //
                //user = _dbContext.Users.FirstOrDefault(a => a.Email.Trim() == model.Email.Trim());
                Request request = new()
                {
                    RequestTypeId = 4,
                    FirstName = model.FamilyFriendFirstName,
                    LastName = model.FamilyFriendLastName,
                    Email = model.FamilyFriendEmail,
                    PhoneNumber = model.FamilyFriendMobile,
                    UserId=user.UserId,
                };
                _dbContext.Add(request);
                await _dbContext.SaveChangesAsync();
                //
                if (model.File != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    };
                    FileInfo fileInfo = new FileInfo(model.File.FileName);
                    string fileName = model.File.FileName + request.RequestId + fileInfo.Extension;
                    string fileNameWithPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        model.File.CopyTo(stream);
                    }
                    RequestWiseFile requestWiseFile = new()
                    {
                        RequestId = request.RequestId,
                        FileName = fileName,
                        CreatedDate = DateTime.Now,
                        Uploder = model.FamilyFriendFirstName + " " + model.FamilyFriendLastName,
                    };
                    _dbContext.Add(requestWiseFile);
                    await _dbContext.SaveChangesAsync();
                }
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
                    //region = _dbContext.Regions.FirstOrDefault(a => a.Name.Trim() == model.State.Trim());
                }
                //
                //request = _dbContext.Requests.FirstOrDefault(a => a.Email.Trim() == model.FamilyFriendEmail.Trim());
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
                    IntYear = DateTime.Now.Year,
                    IntDate = DateTime.Now.Day,
                    StrMonth = DateTime.Now.Month.ToString(),
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
                    //aspNetRole = _dbContext.AspNetRoles.FirstOrDefault(a => a.Name.Trim() == "Patient");
                };
                //
                AspNetUser aspNetUser = _dbContext.AspNetUsers.FirstOrDefault(a => a.Email.Trim() == model.Email.Trim());
                User user = _dbContext.Users.FirstOrDefault(a => a.Email.Trim() == model.Email.Trim());
                if (aspNetUser == null)
                {
                    aspNetUser = new()
                    {
                        UserName = model.FirstName,
                        Email = model.Email,
                        PhoneNumber = model.Mobile,
                        PasswordHash = model.Password,
                        CreatedDate = DateTime.Now
                    };
                    _dbContext.Add(aspNetUser);
                    await _dbContext.SaveChangesAsync();
                    //
                    //aspNetUser = _dbContext.AspNetUsers.FirstOrDefault(a => a.Email.Trim() == model.Email.Trim());
                    user = new()
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
                        IntYear = model.BirthDate.Value.Year,
                        IntDate = model.BirthDate.Value.Day,
                        StrMonth = model.BirthDate.Value.Month.ToString(),
                    };
                    _dbContext.Add(user);
                    await _dbContext.SaveChangesAsync();
                    //
                    //aspNetUser = _dbContext.AspNetUsers.FirstOrDefault(a => a.Email.Trim() == model.Email.Trim());
                    AspNetUserRole aspNetUserRole = new()
                    {
                        UserId = aspNetUser.Id,
                        RoleId = aspNetRole.Id,
                    };
                    _dbContext.Add(aspNetUserRole);
                    await _dbContext.SaveChangesAsync();
                }
                //
                //user= _dbContext.Users.FirstOrDefault(a =>a.Email.Trim() == model.Email.Trim());
                Request request = new()
                {
                    RequestTypeId = 4,
                    FirstName = model.BusinessFirstName,
                    LastName = model.BusinessLastName,
                    Email = model.BusinessEmail,
                    PhoneNumber = model.BusinessMobile,
                    UserId=user.UserId,
                };
                _dbContext.Add(request);
                await _dbContext.SaveChangesAsync();
                //
                if (model.File != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    };
                    FileInfo fileInfo = new FileInfo(model.File.FileName);
                    string fileName = fileInfo.Name + request.RequestId + fileInfo.Extension;
                    string fileNameWithPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        model.File.CopyTo(stream);
                    }
                    RequestWiseFile requestWiseFile = new()
                    {
                        RequestId = request.RequestId,
                        FileName = fileName,
                        CreatedDate = DateTime.Now,
                        Uploder = model.BusinessFirstName + " " + model.BusinessLastName,
                    };
                    _dbContext.Add(requestWiseFile);
                    await _dbContext.SaveChangesAsync();
                }
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
                //business = _dbContext.Businesses.FirstOrDefault(a => a.Name.Trim() == model.BusinessFirstName.Trim());
                //request = _dbContext.Requests.FirstOrDefault(a => a.Email.Trim() == model.BusinessEmail.Trim());
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
                    //region = _dbContext.Regions.FirstOrDefault(a => a.Name.Trim() == model.State.Trim());
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
                    IntYear = DateTime.Now.Year,
                    IntDate = DateTime.Now.Day,
                    StrMonth = DateTime.Now.Month.ToString(),
                };
                _dbContext.Add(requestClient);
                await _dbContext.SaveChangesAsync();
            };
            return View(null);
        }
    }
}
