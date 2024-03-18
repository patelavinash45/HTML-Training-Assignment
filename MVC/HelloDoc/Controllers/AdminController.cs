using AspNetCoreHero.ToastNotification.Abstractions;
using ClosedXML.Excel;
using HelloDoc.Authentication;
using Microsoft.AspNetCore.Mvc;
using Repositories.DataModels;
using Services.Interfaces;
using Services.Interfaces.AdminServices;
using Services.Interfaces.AuthServices;
using Services.ViewModels;
using Services.ViewModels.Admin;
using System.Data;
using System.IO;

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
        private readonly IEncounterService _encounterService;

        public AdminController(INotyfService notyfService,IAdminDashboardService adminDashboardService, IViewCaseService viewCaseService,
                                IViewNotesService viewNotesService, ILoginService loginService, IViewDocumentsServices viewDocumentsServices,
                                IJwtService jwtService, ISendOrderService sendOrderService, IEncounterService encounterService)
        {
            _notyfService = notyfService;
            _loginService = loginService;
            _adminDashboardService = adminDashboardService;
            _viewCaseService = viewCaseService;
            _viewNotesService = viewNotesService;
            _viewDocumentsServices = viewDocumentsServices;
            _jwtService = jwtService;
            _sendOrderService = sendOrderService;
            _encounterService = encounterService;
        }

        public IActionResult LoginPage()
        {
            if (_loginService.isTokenValid(HttpContext,"Admin"))
            {
                return RedirectToAction("Dashboard", "Admin");
            }
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
        public IActionResult EncounterForm()
        {
            int requestId = HttpContext.Session.GetInt32("requestId").Value;
            return View(_encounterService.getEncounterDetails(requestId,true));
        }

        [Authorization("Admin")]
        public IActionResult ViewCase()
        {
            int requestId = HttpContext.Session.GetInt32("requestId").Value;
            return View(_viewCaseService.getRequestDetails(requestId));
        }

        [Authorization("Admin")]
        public IActionResult ViewNotes()
        {
            int requestId = HttpContext.Session.GetInt32("requestId").Value;
            return View(_viewNotesService.GetNotes(requestId));
        }
        
        public IActionResult Agreement(String token)
        {
            Agreement agreement = _adminDashboardService.getUserDetails(token);
            if(agreement != null)
            {
                return View(agreement);
            }
            _notyfService.Error("Link is Invalid");
            return RedirectToAction("PatientSite","Patient");
        }

        [Authorization("Admin")]
        public IActionResult ViewDocument()
        {
            int aspNetUserId = HttpContext.Session.GetInt32("aspNetUserId").Value;
            int requestId = HttpContext.Session.GetInt32("requestId").Value;
            return View(_viewDocumentsServices.getDocumentList(requestId: requestId, aspNetUserId: aspNetUserId));
        }

        [Authorization("Admin")]
        public IActionResult SendOrder()
        {
            int requestId = HttpContext.Session.GetInt32("requestId").Value;
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
            return Json(new { redirect = Url.Action("ViewDocument", "Admin") });
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
            return RedirectToAction("ViewDocument", "Admin");
        }

        public IActionResult SendMail([FromBody] List<int> requestWiseFileIdsList)
        {
            int requestId = HttpContext.Session.GetInt32("requestId").Value;
            if (_viewDocumentsServices.sendFileMail(requestWiseFileIdsList,requestId))
            {
                _notyfService.Success("Successfully Send Mail");
            }
            else
            {
                _notyfService.Error("Faild!");
            }
            return Json(new { redirect = Url.Action("ViewDocument", "Admin") });
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

        public async Task<IActionResult> AssignPopUp(AssignAndTransferPopUp model)
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

        public async Task<IActionResult> TransferPopUp(AssignAndTransferPopUp model)
        {
            if (ModelState.IsValid)
            {
                if (await _viewNotesService.assignRequest(model))
                {
                    _notyfService.Success("Successfully Reuqest Transfer");
                }
                else
                {
                    _notyfService.Error("Request Transfer Faild!");
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

        [HttpGet] 
        public async Task<JsonResult> ClearPopUp(int requestId)
        {
            if (await _viewNotesService.clearRequest(requestId))
            {
                _notyfService.Success("Successfully Reuqest Clear");
            }
            else
            {
                _notyfService.Error("Request Clear Faild !!");
            }
            return Json(new { redirect = Url.Action("Dashboard", "Admin") });
        }

        public IActionResult SendAgreementPopUp(Agreement model)
        {
            if (ModelState.IsValid)
            {
                if (_viewNotesService.sendAgreement(model))
                {
                    _notyfService.Success("Successfully Send");
                }
                else
                {
                    _notyfService.Error("Agreement Send Faild!");
                }
                return RedirectToAction("Dashboard", "Admin");
            }
            return View(model);
        }

        public async Task<IActionResult> AgreementAgree(Agreement model)
        {
            await _viewNotesService.agreementAgree(model);
            return RedirectToAction("Dashboard", "Patient");
        }

        public async Task<IActionResult> AgreementDeclined(Agreement model)
        {
            await _viewNotesService.agreementDeclined(model);
            return RedirectToAction("Dashboard", "Patient");
        }

        [HttpGet] 
        public JsonResult SetRequestId(int requestId, String actionName)
        {
            HttpContext.Session.SetInt32("requestId", requestId);
            return Json(new { redirect = Url.Action(actionName, "Admin") });
        }

        [HttpGet]  // SendAgreementPopUp
        public JsonResult GetEmailAndMobileNumber(int requestId)
        {
            return Json(_adminDashboardService.getRequestClientEmailAndMobile(requestId));
        }

        [HttpGet] // Assigncase and TransfercasePopUp
        public JsonResult GetPhysicians(int regionId)
        {
            return Json(_adminDashboardService.getPhysiciansByRegion(regionId));
        }

        [HttpGet] // Send Order
        public JsonResult GetBussinesses(int professionId)
        {
            return Json(_sendOrderService.getBussinessByProfession(professionId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewDocument(ViewDocument model)
        {
            if (ModelState.IsValid)
            {
                String firstname = HttpContext.Session.GetString("firstName");
                String lastName = HttpContext.Session.GetString("lastName");
                int requestId = HttpContext.Session.GetInt32("requestId").Value;
                if (await _viewDocumentsServices.uploadFile(model,firstName:firstname,lastName: lastName,requestId) > 0)
                {
                    _notyfService.Success("Successfully File Added.");
                    return RedirectToAction("ViewDocument", "Admin");
                }
            }
            _notyfService.Warning("Add Required Field");
            return RedirectToAction("viewDocument", "Admin");
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
        public async Task<IActionResult> EncounterForm(EncounterForm model)
        {
            if (ModelState.IsValid)
            {
                int requestId = HttpContext.Session.GetInt32("requestId").Value;
                if (await _encounterService.updateEncounter(model,requestId))
                {
                    _notyfService.Success("Successfully Updated");
                }
                else
                {
                    _notyfService.Error("Update Faild !!");
                }
                return RedirectToAction("Dashboard", "Admin");
            }
            _notyfService.Warning("Add Required Field.");
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
                    string token = _jwtService.genrateJwtToken(user);
                    CookieOptions cookieOptions = new CookieOptions()
                    {
                        Secure = true,
                        Expires = DateTime.Now.AddMinutes(20),
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
                    _notyfService.Success("Successfully Updated");
                }
                else
                {
                    _notyfService.Error("Update Faild");
                }
                return RedirectToAction("ViewCase", "Admin");
            }
            return View(model);
        }

        [HttpGet]   // Export All Data 
        public IActionResult ExportAllData()
        {
            DataTable dataTable = _adminDashboardService.exportAllData();
            using (XLWorkbook wb = new XLWorkbook())
            { 
                wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream); 
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AllData.xlsx");
                }
            }
        }

        [HttpGet]   // Export All Data 
        public IActionResult ExportData(int pageNo, String status, int type, String searchElement)
        {
            DataTable dataTable = _adminDashboardService.exportData(pageNo, status, type, searchElement);
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Data.xlsx");
                }
            }
        }

        [HttpGet] // Dashboard 
        public IActionResult GetTablesData(String status,int pageNo,String partialViewName)
        {
           TableModel tableModel= _adminDashboardService.GetNewRequest(status, pageNo);
           return PartialView(partialViewName, tableModel);
        }

        [HttpGet] // search on Dashboard 
        public IActionResult Search(String searchElement,String status, String partialViewName, int pageNo, int type)
        {
            TableModel tableModel = _adminDashboardService.patientSearch(searchElement, status,pageNo,type);
            return tableModel.TableDatas.Count != 0 ? PartialView(partialViewName, tableModel) : PartialView("_NoTableDataFound");
        }

        [HttpPost]
        public async Task<IActionResult> ViewNotes(ViewNotes model)
        {
            if(ModelState.IsValid)
            {
                int requestId = HttpContext.Session.GetInt32("requestId").Value;
                if (await _viewNotesService.addAdminNotes(model.NewAdminNotes,requestId))
                {
                    _notyfService.Success("Successfully Notes Added");
                }
                else
                {
                    _notyfService.Error("Add Notes Faild");
                }
            }
            return RedirectToAction("ViewNotes", "Admin");
        }

        [HttpGet] // Send Order
        public HealthProfessional GetBussinessData(int venderId)
        {
            return _sendOrderService.getBussinessData(venderId);
        }
    }
}
