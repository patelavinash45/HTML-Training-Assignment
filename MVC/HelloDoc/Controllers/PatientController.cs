using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.IO.Compression;
using System.Net;
using System.Text;
using Repositories.ViewModels;
using HelloDoc.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
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

        public IActionResult NewPassword(String token, int aspNetUserId, string time)
        {
            return View(_resetPasswordService.validatePasswordLink(token, aspNetUserId, time));
        }

        public IActionResult RequestForSomeOne(int id)
        {
            User user = _dbContext.Users.FirstOrDefault(a => a.AspNetUserId == id);
            DashboardHeader dashboardHeader = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                AspNetUserId = id,
            };
            AddRequestByPatient addRequestForMe = new()
            {
                Header = dashboardHeader,
            };
            return View(addRequestForMe);
        }

        public IActionResult RequestForMe(int id)
        {
            User user = _dbContext.Users.FirstOrDefault(a => a.AspNetUserId == id);
            DateTime birthDay = DateTime.Parse(user.IntYear + "-" + user.StrMonth + "-" + user.IntDate);
            DashboardHeader dashboardHeader = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                AspNetUserId = id,
            };
            AddRequestByPatient addRequestForMe = new() 
            { 
                Header= dashboardHeader,
                FirstName=user.FirstName, 
                LastName=user.LastName,
                BirthDate =birthDay,
                Mobile = user.Mobile,
                Email= user.Email,
            };
            return View(addRequestForMe);
        }

        public IActionResult ViewProfile(int id)
        { 
            return View(_viewProfileService.getProfileDetails(id));
        }

        public IActionResult ViewDocument(int id)
        {
            return View(_viewDocumentsServices.getDocumentList(requestId: id,aspNetUserId: Int32.Parse(Request.Cookies["AspNetUserId"])));
        }

        public IActionResult Dashboard(int id)
        {
            CookieOptions options = new CookieOptions();
            options.Secure = true;
            options.Expires = DateTime.Now.AddDays(60);
            Response.Cookies.Append("AspNetUserId", id.ToString(), options);
            return View(_dashboardService.GetUsersMedicalData(id));
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public Task<bool> IsEmailExists(string email)
        {
            AspNetUser aspNetUser = _dbContext.AspNetUsers.FirstOrDefault(a => a.Email.Trim() == email.Trim());
            if (aspNetUser == null)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }


        [HttpGet]
        public async Task<IActionResult> CheckEmailExists(string email)
        {
            var emailExists = await IsEmailExists(email);
            return Json(new { emailExists });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginPage(PatientLogin model)
        {
            if (ModelState.IsValid)
            {
                int result = _loginService.auth(model);
                if (result == 0)
                {
                    _notyfService.Error("Invalid credentials");
                    return View(null);
                }
                else
                {
                    _notyfService.Success("Successfully Login");
                    return RedirectToAction("Dashboard", "Patient", new { id = result });
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
            if(await _viewProfileService.updatePatientProfile(model))
            {
                _notyfService.Success("Successfully Updated");
            }
            else
            {
                _notyfService.Error("Faild!");
            }
            return RedirectToAction("ViewProfile", "Patient", new { id = Request.Cookies["AspNetUserId"] });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            _resetPasswordService.resetPasswordLinkSend(model.Email);
            return RedirectToAction("PatientSite", "Patient");
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
            ModelState.Remove("Header");
            if (ModelState.IsValid)
            {
                User user = _dbContext.Users.FirstOrDefault(a => a.AspNetUserId == Int32.Parse(Request.Cookies["AspNetUserId"]));
                if (user != null)
                {
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
                    //request = _dbContext.Requests.FirstOrDefault(a => a.Email.Trim() == model.Email.Trim());
                    if (model.File != null)
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        };
                        FileInfo fileInfo = new FileInfo(model.File.FileName);
                        string fileName = request.RequestId + "_" + fileInfo.Name;
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
                            Uploder = model.FirstName + " " + model.LastName,
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
                    _notyfService.Success("Successfully Request Added");
                    return RedirectToAction("Dashboard", "Patient", new { id = Int32.Parse(Request.Cookies["AspNetUserId"]) });
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestForSomeOne(AddRequestByPatient model)
        {
            ModelState.Remove("Header");
            if (ModelState.IsValid)
            {
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
                    AspNetRole aspNetRole = _dbContext.AspNetRoles.FirstOrDefault(a => a.Name.Trim() == "Patient");
                    AspNetUserRole aspNetUserRole = new()
                    {
                        UserId = aspNetUser.Id,
                        RoleId = aspNetRole.Id,
                    };
                    _dbContext.Add(aspNetUserRole);
                    await _dbContext.SaveChangesAsync();
                }
                //
                User user2 = _dbContext.Users.FirstOrDefault(a => a.Email.Trim() == model.Email.Trim());
                Request request = new()
                {
                    RequestTypeId = 4,
                    FirstName = user2.FirstName,
                    LastName = user2.LastName,
                    Email = user2.Email,
                    PhoneNumber = user2.Mobile,
                    UserId = user.UserId,
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
                        Uploder = user2.FirstName + " " + user2.LastName,
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
                _notyfService.Success("Successfully Request Added");
                return RedirectToAction("Dashboard", "Patient", new { id = Int32.Parse(Request.Cookies["AspNetUserId"]) });
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PatientRequest(AddPatientRequest model)
        {
            if (ModelState.IsValid)
            {
                await _addRequestService.addPatientRequest(model);
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
                    FirstName = model.ConciergeFirstName,
                    LastName = model.ConciergeLastName,
                    Email = model.ConciergeEmail,
                    PhoneNumber = model.ConciergeMobile,
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
                        Uploder=model.ConciergeFirstName+" "+model.ConciergeLastName,
                    };
                    _dbContext.Add(requestWiseFile);
                    await _dbContext.SaveChangesAsync();
                }
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
                    //region = _dbContext.Regions.FirstOrDefault(a => a.Name.Trim() == model.ConciergeState.Trim());
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
                //concierge = _dbContext.Concierges.FirstOrDefault(a => a.ConciergeName.Trim() == model.ConciergeFirstName.Trim());
                //request = _dbContext.Requests.FirstOrDefault(a => a.Email.Trim() == model.ConciergeEmail.Trim());
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
                    //region = _dbContext.Regions.FirstOrDefault(a => a.Name.Trim() == model.State.Trim());
                }
                //
                //request = _dbContext.Requests.FirstOrDefault(a => a.Email.Trim() == model.ConciergeEmail.Trim());
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
