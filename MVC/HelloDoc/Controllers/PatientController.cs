﻿using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using HelloDoc.Authentication;
using Microsoft.AspNetCore.Mvc;
using Repositories.ViewModels;
using Services.Interfaces;
using Services.Interfaces.AuthServices;
using Services.Interfaces.PatientServices;

namespace HelloDoc.Controllers
{
    public class PatientController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly ILoginService _loginService;
        private readonly IPatientDashboardService _dashboardService;
        private readonly IAddRequestService _addRequestService;
        private readonly IViewProfileService _viewProfileService;
        private readonly IResetPasswordService _resetPasswordService;
        private readonly IViewDocumentsServices _viewDocumentsServices;
        private readonly IJwtService _jwtService;

        public PatientController(INotyfService notyfService,ILoginService loginService ,IPatientDashboardService dashboardService,
                                 IAddRequestService addRequestService,IViewProfileService viewProfileService, IResetPasswordService resetPasswordService,
                                 IViewDocumentsServices viewDocumentsServices,IJwtService jwtService)
        {
            _notyfService = notyfService;
            _loginService = loginService;
            _dashboardService = dashboardService;
            _addRequestService = addRequestService;
            _viewProfileService= viewProfileService;
            _resetPasswordService= resetPasswordService;
            _viewDocumentsServices = viewDocumentsServices;
            _jwtService = jwtService;
        }

        [Route("/")]
        public IActionResult PatientSite()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            _notyfService.Warning("Access Denied !!");
            return RedirectToAction("PatientSite", "Patient");
        }

        public IActionResult WrongLogInPage()
        {
            _notyfService.Information("LogIn To Access Page");
            return RedirectToAction("LoginPage", "Patient");
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

        [Authorization("Patient")]
        public IActionResult RequestForSomeOne()
        {
            int aspNetUserId = HttpContext.Session.GetInt32("aspNetUserId").Value;
            return View(_addRequestService.getModelForRequestForSomeoneelse(aspNetUserId));
        }

        [Authorization("Patient")]
        public IActionResult RequestForMe()
        {
            int aspNetUserId = HttpContext.Session.GetInt32("aspNetUserId").Value;
            return View(_addRequestService.getModelForRequestByMe(aspNetUserId));
        }

        [Authorization("Patient")]
        public IActionResult ViewProfile()
        {
            int aspNetUserId = HttpContext.Session.GetInt32("aspNetUserId").Value;
            return View(_viewProfileService.getProfileDetails(aspNetUserId));
        }

        [Authorization("Patient")]
        public IActionResult ViewDocument()
        {
            int aspNetUserId = HttpContext.Session.GetInt32("aspNetUserId").Value;
            int requestId = HttpContext.Session.GetInt32("requestId").Value;
            return View(_viewDocumentsServices.getDocumentList(requestId: requestId,aspNetUserId: aspNetUserId));
        }

        [Authorization("Patient")]
        public IActionResult Dashboard()
        {
            int aspNetUserId = HttpContext.Session.GetInt32("aspNetUserId").Value;
            return View(_dashboardService.GetUsersMedicalData(aspNetUserId));
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("aspNetUserId");
            HttpContext.Session.Remove("role");
            Response.Cookies.Delete("jwtToken");
            return RedirectToAction("LoginPage", "Patient");
        }

        [HttpGet]
        public JsonResult SetRequestId(int requestId)
        {
            HttpContext.Session.SetInt32("requestId", requestId);
            return Json(new { redirect = Url.Action("ViewDocument","Patient") });
        }

        [HttpGet]
        public JsonResult CheckEmailExists(string email)
        {
            var emailExists = _addRequestService.IsEmailExists(email);
            return Json(new { emailExists });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginPage(Login model)
        {
            if (ModelState.IsValid)
            {
                int aspNetUserId = _loginService.auth(model,1);
                if (aspNetUserId == 0)
                {
                    _notyfService.Error("Invalid credentials");
                    return View(null);
                }
                else
                {
                    HttpContext.Session.SetInt32("aspNetUserId", aspNetUserId);
                    HttpContext.Session.SetString("role", "Patient");
                    _notyfService.Success("Successfully Login");
                    string token = _jwtService.GenerateJwtToken(role: "Patient",aspNetUserId : aspNetUserId);
                    CookieOptions cookieOptions = new CookieOptions()
                    {
                        Secure = true,
                        Expires = DateTime.UtcNow.AddMinutes(20),
                    };
                    Response.Cookies.Append("jwtToken", token, cookieOptions);
                    //HttpContext.Session.SetString("jwtToken", token);
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
                    return RedirectToAction("PatientSite", "Patient");
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
            if (ModelState.IsValid)
            {
                if(await _viewDocumentsServices.uploadFile(model)>0)
                {
                    return RedirectToAction("ViewDocument", "Patient", new { id = model.RequestId });
                }
            }
            _notyfService.Warning("Please, Add Required Field.");
            return View(model);
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
            _notyfService.Warning("Please, Add Required Field.");
            return View(model);
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
            _notyfService.Warning("Please, Add Required Field.");
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
            _notyfService.Warning("Please, Add Required Field.");
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
            _notyfService.Warning("Please, Add Required Field.");
            return View(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FamilyFriendRequest(AddFamilyRequest model)
        {
            if (ModelState.IsValid)
            {
                if (await _addRequestService.addFamilyFriendRequest(model))
                {
                    _notyfService.Success("Successfully Request Added");
                    return RedirectToAction("LoginPage", "Patient");
                }
                else
                {
                    _notyfService.Error("Add Request Faild");
                }
            }
            _notyfService.Warning("Please, Add Required Field.");
            return View(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BusinessRequest(AddBusinessRequest model)
        {
            if (ModelState.IsValid)
            {
                if (await _addRequestService.addBusinessRequest(model))
                {
                    _notyfService.Success("Successfully Request Added");
                    return RedirectToAction("LoginPage", "Patient");
                }
                else
                {
                    _notyfService.Error("Add Request Faild");
                }
            }
            _notyfService.Warning("Please, Add Required Field.");
            return View(null);
        }
    }
}
