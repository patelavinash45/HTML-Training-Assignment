﻿using HelloDoc.DataContext;
using HelloDoc.DataModels;
using HelloDoc.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult ViewDocument(int id)
        {
            List<RequestWiseFile> requestWiseFiles = _dbContext.RequestWiseFiles.Where(a => a.RequestId == id).ToList();
            List<ViewDocument> viewDocuments = new List<ViewDocument>();
            for(int i=0;i< requestWiseFiles.Count; i++)
            { 
                String uploader;
                Request request = _dbContext.Requests.FirstOrDefault(a => a.RequestId == id);
                uploader=request.FirstName+" "+request.LastName;
                RequestWiseFile requestWiseFile = _dbContext.RequestWiseFiles.FirstOrDefault(a => a.RequestId == request.RequestId);
                ViewDocument viewDocument = new()
                {
                    FileName=requestWiseFile.FileName,
                    Uploder=uploader,
                    Day= requestWiseFile.CreatedDate.Day,
                    Month=requestWiseFile.CreatedDate.Month,
                    Year=requestWiseFile.CreatedDate.Year,
                    PhysicianId=requestWiseFile.PhysicianId,   
                    AdminId=requestWiseFile.AdminId,
                };
                viewDocuments.Add(viewDocument);
            }
            return View(viewDocuments);
        }

        public IActionResult Dashboard(int id)
        {
            User user = _dbContext.Users.FirstOrDefault(a => a.AspNetUserId == id);
            List<Request> requests = _dbContext.Requests.Where(a => a.UserId == user.UserId).ToList();
            List<RequestClient> requestClient = new List<RequestClient>() { };
            for (int i = 0; i < requests.Count; i++)
            {
                requestClient.Add(_dbContext.RequestClients.FirstOrDefault(a => a.RequestId == requests[i].RequestId));
            }
            List<Dashboard> dashboards = new List<Dashboard>() { };
            for (int i = 0; i < requests.Count; i++)
            {
                List<RequestWiseFile> requestWiseFiles = _dbContext.RequestWiseFiles.Where(a => a.RequestId == requests[i].RequestId).ToList();
                Dashboard dashboard = new()
                {
                    RequestId= requests[i].RequestId,
                    FirstName = requestClient[i].FirstName,
                    LastName = requestClient[i].LastName,
                    StrMonth = requestClient[i].StrMonth,
                    IntYear = requestClient[i].IntYear,
                    IntDate = requestClient[i].IntDate,
                    Status = requestClient[i].Status,
                    Document = requestWiseFiles.Count,
                };
                dashboards.Add(dashboard);
            }
            return View(dashboards);
        }
            
        [HttpPost]
        public async Task<bool> UploadFile(IFormFile file)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            };
            FileInfo fileInfo = new FileInfo(file.FileName);
            string fileName = fileInfo.Name + "40" + fileInfo.Extension;
            string fileNameWithPath = Path.Combine(path, fileName);
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            RequestWiseFile requestWiseFile = new()
            {
                RequestId = 40,
                FileName = fileName,
                CreatedDate = DateTime.Now,
            };
            _dbContext.Add(requestWiseFile);
            await _dbContext.SaveChangesAsync();
            return true;
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
            if(ModelState.IsValid)
            {
                AspNetUser aspNetUser = _dbContext.AspNetUsers.FirstOrDefault(a => a.Email.Trim() == model.Email.Trim());
                if (aspNetUser == null)
                {
                    ViewBag.error = 1;
                    return View(null);
                }
                else if (aspNetUser.PasswordHash != model.PasswordHash)
                {
                    ViewBag.error = 2;
                    return View(null);
                }
                else
                {
                    return RedirectToAction("Dashboard", "Patient", new { id = aspNetUser.Id });
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
                    //aspNetRole = _dbContext.AspNetRoles.FirstOrDefault(a => a.Name.Trim() == "Patient");
                 };
                //
                AspNetUser aspNetUser = _dbContext.AspNetUsers.FirstOrDefault(a => a.Email.Trim() == model.Email.Trim());
                User user = _dbContext.Users.FirstOrDefault(a => a.Email.Trim() == model.Email.Trim());
                if(aspNetUser == null)
                {
                    aspNetUser= new()
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
                //user = _dbContext.Users.FirstOrDefault(a => a.Email.Trim() == user.Email.Trim());
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
                    string fileName = request.RequestId + "_"+ fileInfo.Name;
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
