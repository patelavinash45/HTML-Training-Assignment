using AspNetCoreHero.ToastNotification.Abstractions;
using HelloDoc.Authentication;
using Microsoft.AspNetCore.Mvc;
using Repositories.DataModels;
using Services.ViewModels;
using Services.ViewModels.Admin;
using Services.Implementation.AuthServices;
using Services.Interfaces;
using Services.Interfaces.AdminServices;
using Services.Interfaces.AuthServices;

namespace HelloDoc.Controllers
{
    public class AdminController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly ILoginService _loginService;
        private readonly IAdminDashboardService _adminDashboardService;
        private readonly IViewCaseService _viewCaseService;
        private readonly IViewNotesService _viewNotesService;
        private readonly IViewDocumentsServices _viewDocumentsServices;
        private readonly IJwtService _jwtService;
        private readonly ISendOrderService _sendOrderService;

        public AdminController(INotyfService notyfService,IAdminDashboardService adminDashboardService, IViewCaseService viewCaseService,
                                IViewNotesService viewNotesService, ILoginService loginService, IViewDocumentsServices viewDocumentsServices,
                                         IJwtService jwtService, ISendOrderService sendOrderService)
        {
            _notyfService = notyfService;
            _loginService = loginService;
            _adminDashboardService = adminDashboardService;
            _viewCaseService = viewCaseService;
            _viewNotesService = viewNotesService;
            _viewDocumentsServices = viewDocumentsServices;
            _jwtService = jwtService;
            _sendOrderService = sendOrderService;
        }

        public IActionResult WrongLogInPage()
        {
            _notyfService.Information("LogIn To Access Page");
            return RedirectToAction("LoginPage", "Admin");
        }

        public IActionResult LoginPage()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("aspNetUserId");
            HttpContext.Session.Remove("role");
            Response.Cookies.Delete("jwtToken");
            return RedirectToAction("LoginPage", "Admin");
        }

        [Authorization("Admin")]
        public IActionResult Dashboard()
        {
            int aspNetUseId = HttpContext.Session.GetInt32("aspNetUserId").Value;
            return aspNetUseId > 0 ? View(_adminDashboardService.getallRequests(aspNetUseId)) : View(null);
        }

        [Authorization("Admin")]
        public IActionResult ViewCase(int requestId)
        {
            return View(_viewCaseService.getRequestDetails(requestId));
        }

        [Authorization("Admin")]
        public IActionResult ViewNotes(int requestId)
        {
            return View(_viewNotesService.GetNotes(requestId));
        }

        [Authorization("Admin")]
        public IActionResult ViewDocument(int requestId)
        {
            int aspNetUserId = HttpContext.Session.GetInt32("aspNetUserId").Value;
            return View(_viewDocumentsServices.getDocumentList(requestId: requestId, aspNetUserId: aspNetUserId));
        }

        [Authorization("Admin")]
        public IActionResult SendOrder(int requestId)
        {
            return View(_sendOrderService.getSendOrderDetails(requestId));
        }

        public async Task<JsonResult> DeleteAllFiles([FromBody]List<int> requestWiseFileIdsList)
        {
            int requestId = await _viewDocumentsServices.deleteAllFile(requestWiseFileIdsList);
            if (requestId > 0)
            {
                _notyfService.Success("Successfully File Deleted");
            }
            else
            {
                _notyfService.Error("Faild!");
            }
            return Json(new { redirect = Url.Action("ViewDocument", "Admin", new { requestId = requestId }) });
        }

        public async Task<IActionResult> DeleteFile(int requestWiseFileId)
        {
            int requestId = await _viewDocumentsServices.deleteFile(requestWiseFileId);
            if (requestId > 0)
            {
                _notyfService.Success("Successfully File Deleted");
            }
            else
            {
                _notyfService.Error("Faild!");
            }
            return RedirectToAction("ViewDocument", "Admin",new { requestId = requestId});
        }

        public async Task<IActionResult> SendMail([FromBody] List<int> requestWiseFileIdsList)
        {
            int requestId = await _viewDocumentsServices.sendFileMail(requestWiseFileIdsList);
            if (requestId > 0)
            {
                _notyfService.Success("Successfully Send Mail");
            }
            else
            {
                _notyfService.Error("Faild!");
            }
            return RedirectToAction("ViewDocument", "Admin", new { requestId = requestId });
        }

        public async Task<IActionResult> CancelPopUp(CancelPopUp model)
        {
            if(ModelState.IsValid)
            {
                if (await _viewNotesService.cancleRequest(model))
                {
                    _notyfService.Success("Successfully Reuqest Cancel");
                }
                else
                {
                    _notyfService.Error("Request Cancel Faild!");
                }
                return RedirectToAction("Dashboard", "Admin");
            }
            return View(model);
        }

        public async Task<IActionResult> AssignPopUp(AssignPopUp model)
        {
            if (ModelState.IsValid)
            {

                if (await _viewNotesService.assignRequest(model))
                {
                    _notyfService.Success("Successfully Reuqest Assign");
                }
                else
                {
                    _notyfService.Error("Request Assign Faild!");
                }
                return RedirectToAction("Dashboard", "Admin");
            }
            return View(model);
        }

        public async Task<IActionResult> BlockPopUp(BlockPopUp model)
        {
            if (ModelState.IsValid)
            {

                if (await _viewNotesService.blockRequest(model))
                {
                    _notyfService.Success("Successfully Reuqest Block");
                }
                else
                {
                    _notyfService.Error("Request Block Faild!");
                }
                return RedirectToAction("Dashboard", "Admin");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewDocument(ViewDocument model)
        {
            if (ModelState.IsValid)
            {
                String firstname = HttpContext.Session.GetString("firstName");
                String lastName = HttpContext.Session.GetString("lastName");
                if (await _viewDocumentsServices.uploadFile(model,firstName:firstname,lastName: lastName) > 0)
                {
                    return RedirectToAction("ViewDocument", "Admin", new { requestId = model.RequestId });
                }
            }
            _notyfService.Warning("Please, Add Required Field.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendOrder(SendOrder model)
        {
            if (ModelState.IsValid)
            {

                if (await _sendOrderService.addOrderDetails(model) > 0)
                {
                    _notyfService.Success("Successfully Order Added");
                }
                else
                {
                    _notyfService.Error("Order Faild !!");
                }
                return RedirectToAction("Dashboard", "Admin");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginPage(Login model)
        {
            if (ModelState.IsValid)
            {
                UserDataModel user = _loginService.auth(model, 2);
                if (user == null)
                {
                    _notyfService.Error("Invalid credentials");
                    return View(null);
                }
                else
                {
                    HttpContext.Session.SetInt32("aspNetUserId", user.AspNetUserId);
                    HttpContext.Session.SetString("role", user.UserType);
                    HttpContext.Session.SetString("firstName", user.FirstName);
                    HttpContext.Session.SetString("lastName", user.LastName);
                    string token = _jwtService.GenerateJwtToken(role: user.UserType, aspNetUserId: user.AspNetUserId);
                    CookieOptions cookieOptions = new CookieOptions()
                    {
                        Secure = true,
                        Expires = DateTime.UtcNow.AddMinutes(20),
                    };
                    Response.Cookies.Append("jwtToken", token, cookieOptions);
                    //HttpContext.Session.SetString("jwtToken", token);
                    _notyfService.Success("Successfully Login");
                    return RedirectToAction("Dashboard", "Admin");
                }
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> ViewCase(ViewCase model) 
        {
            if(ModelState.IsValid)
            {
                if (await _viewCaseService.updateRequest(model))
                {
                    _notyfService.Success("Successfully Request Updated");
                }
                else
                {
                    _notyfService.Error("Update Request Faild");
                }
                return RedirectToAction("ViewCase", "Admin", new { requestId = model.RequestId });
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult GetTablesData(String status,int pageNo)
        {
           TableModel tableModel= _adminDashboardService.GetNewRequest(status, pageNo);
           switch(status)
            {
                case "New": return PartialView("_NewTable", tableModel); 
                case "Pending": return PartialView("_PendingTable", tableModel);
                case "Active": return PartialView("_ActiveTable", tableModel);
                case "Conclude": return PartialView("_ConcludeTable", tableModel);
                case "Close": return PartialView("_CloseTable", tableModel);
                case "Unpaid": return PartialView("_UnpaidTable", tableModel);
                default: return View();
            }
        }

        [HttpGet]
        public IActionResult SearchPatient(String patientName,String status)
        {
            TableModel tableModel = _adminDashboardService.searchPatient(patientName);
            switch (status)
            {
                case "New": return PartialView("_NewTable", tableModel);
                case "Pending": return PartialView("_PendingTable", tableModel);
                case "Active": return PartialView("_ActiveTable", tableModel);
                case "Conclude": return PartialView("_ConcludeTable", tableModel);
                case "Close": return PartialView("_CloseTable", tableModel);
                case "Unpaid": return PartialView("_UnpaidTable", tableModel);
                default: return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ViewNotes(ViewNotes model)
        {
            if(ModelState.IsValid)
            {
                if (await _viewNotesService.addAdminNotes(model))
                {
                    _notyfService.Success("Successfully Notes Added");
                }
                else
                {
                    _notyfService.Error("Add Notes Faild");
                }
                return RedirectToAction("ViewNotes", "Admin", new { requestId = model.RequestId });
            }
            return View();
        }

        [HttpGet]
        public HealthProfessional GetBussinessData(int venderId)
        {
            return _sendOrderService.getBussinessData(venderId);
        }
    }
}
