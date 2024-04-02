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
        private readonly ICloseCaseService _closeCaseService;
        private readonly IViewProfileService _viewProfileService;
        private readonly IProvidersService _providersService;
        private readonly IAccessService _accessService;

        public AdminController(INotyfService notyfService,IAdminDashboardService adminDashboardService, IViewCaseService viewCaseService,
                                IViewNotesService viewNotesService, ILoginService loginService, IViewDocumentsServices viewDocumentsServices,
                                IJwtService jwtService, ISendOrderService sendOrderService, IEncounterService encounterService, 
                                ICloseCaseService closeCaseService, IViewProfileService viewProfileService,
                                IProvidersService providersService,IAccessService accessService)
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
            _closeCaseService = closeCaseService;
            _viewProfileService = viewProfileService;   
            _providersService = providersService;
            _accessService = accessService; 
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

        [Authorization("Admin")]
        public IActionResult CreateRequest()
        {
            return View(null);
        }

        [Authorization("Admin")]
        public IActionResult CreateProvider()
        {
            return View(_providersService.getCreateProvider());
        }

        [Authorization("Admin")]
        public IActionResult CreateAdmin()
        {
            return View(_accessService.GetAdminCreaateAndProfile());
        }

        [Authorization("Admin")]
        public IActionResult Providers()
        {
            return View(_providersService.getProviders(regionId: 0));
        }

        [Authorization("Admin")]
        public IActionResult Access()
        {
            return View(_accessService.getAccessData());
        }

        [Authorization("Admin")]
        public IActionResult CreateRole()
        {
            return View(_accessService.getCreateRole());
        }

        [Authorization("Admin")]
        public IActionResult ViewProfile()
        {
            int aspNetUseId = HttpContext.Session.GetInt32("aspNetUserId").Value;
            return View(_viewProfileService.GetAdminViewProfile(aspNetUseId));
        }

        [Authorization("Admin")]
        public IActionResult ProviderScheduling()
        {
            return View(_providersService.getProviderSchedulingData());
        }

        [Authorization("Admin")]
        public IActionResult CloseCase()
        {
            int requestId = HttpContext.Session.GetInt32("requestId").Value;
            return View(_closeCaseService.getDaetails(requestId));
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

        public async Task<JsonResult> DeleteAllFiles([FromBody]List<int> requestWiseFileIdsList)   // delete all seleted file - view documents
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

        public async Task<IActionResult> DeleteFile(int requestWiseFileId)   /// delete perticuler one file - view documents
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

        public IActionResult SendMail([FromBody] List<int> requestWiseFileIdsList)  /// send mail from view document
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
                if (_viewNotesService.sendAgreement(model, HttpContext))
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

        public IActionResult RequestSupport(RequestSupport model)  //// request support on dashboard
        {
            if (ModelState.IsValid)
            {
                if (_adminDashboardService.RequestSupport(model))
                {
                    _notyfService.Success("Successfully Send");
                }
                else
                {
                    _notyfService.Error("Faild!");
                }
                return RedirectToAction("Dashboard", "Admin");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShift(CreateShift model)  //// request support on dashboard
        {
            if (ModelState.IsValid)
            {
                int aspNetUseId = HttpContext.Session.GetInt32("aspNetUserId").Value;
                if (await _providersService.createShift(model,aspNetUseId))
                {
                    _notyfService.Success("Successfully Send");
                }
                else
                {
                    _notyfService.Error("Faild!");
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

        [HttpGet]  /// set requestId in session
        public JsonResult SetRequestId(int requestId, String actionName)
        {
            HttpContext.Session.SetInt32("requestId", requestId);
            return Json(new { redirect = Url.Action(actionName, "Admin") });
        }

        [HttpGet]  ////  SendAgreementPopUp
        public JsonResult GetEmailAndMobileNumber(int requestId)
        {
            return Json(_adminDashboardService.getRequestClientEmailAndMobile(requestId));
        }

        [HttpGet] //// Assigncase and TransfercasePopUp
        public JsonResult GetPhysicians(int regionId)
        {
            return Json(_adminDashboardService.getPhysiciansByRegion(regionId));
        }

        [HttpGet] //// Send Order
        public JsonResult GetBussinesses(int professionId)
        {
            return Json(_sendOrderService.getBussinessByProfession(professionId));
        }

        [HttpGet] //// Region Filter on Provider page
        public IActionResult RegionFilter(int regionId)
        {
            return PartialView("_ProviderTable", _providersService.getProviders(regionId: regionId).providers);
        }

        [HttpGet] //// change checkboc menus on create role page
        public IActionResult ChangeMenusByRole(int roleId)
        {
            return PartialView("_CreateRoleCheckBox", _accessService.getMenusByRole(roleId));
        }

        [HttpGet] //// create roles
        public async Task<IActionResult> CreateRoles(String data)
        {
            if (await _accessService.createRole(data))
            {
                _notyfService.Success("Successfully Role Created");
            }
            else
            {
                _notyfService.Error("Faild !!");
            }
            return Json(new { redirect = Url.Action("Access", "Admin") });
        }

        public async Task<IActionResult> DeteteRole(int roleId)  ////   delete role - access page
        {
            if (await _accessService.delete(roleId))
            {
                _notyfService.Success("Successfully Role Created");
            }
            else
            {
                _notyfService.Error("Faild !!");
            }
            return RedirectToAction("Access", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  /////  sendlink ---  Dashboard
        public IActionResult SendLink(SendLink model) 
        {
            if (ModelState.IsValid)
            {
                if (_adminDashboardService.SendRequestLink(model,HttpContext))
                {
                    _notyfService.Success("Successfully Link Send");
                }
                else
                {
                    _notyfService.Error("Link Send Faild !!");
                }
            }
            return RedirectToAction("Dashboard", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  /////  create request ---  Dashboard
        public async Task<IActionResult> CreateRequest(CreateRequest model)
        {
            if (ModelState.IsValid)
            {
                int aspNetUserId = HttpContext.Session.GetInt32("aspNetUserId").Value;
                if (await _adminDashboardService.createRequest(model, aspNetUserId))
                {
                    _notyfService.Success("Successfully Request Added");
                }
                else
                {
                    _notyfService.Error("Faild!");
                }
                return RedirectToAction("Dashboard", "Admin");
            }
            _notyfService.Warning("Add Required Field.");
            return View(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  /////  create provider ---  provider page
        public async Task<IActionResult> CreateProvider(CreateProvider model)
        {
            if (ModelState.IsValid)
            {
                if (await _providersService.createProvider(model))
                {
                    _notyfService.Success("Successfully Provider Created");
                }
                else
                {
                    _notyfService.Error("Faild!");
                }
                return RedirectToAction("Providers", "Admin");
            }
            CreateProvider createProvider = _providersService.getCreateProvider();
            model.Roles = createProvider.Roles;
            model.Regions = createProvider.Regions;
            _notyfService.Warning("Add Required Field.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  /////  create Admin ---  dashboard 
        public async Task<IActionResult> CreateAdmin(AdminCreaateAndProfile model)
        {
            if (ModelState.IsValid)
            {
                if (await _accessService.createAdmin(model))
                {
                    _notyfService.Success("Successfully Admin Created");
                }
                else
                {
                    _notyfService.Error("Faild!");
                }
                return RedirectToAction("Dashboard", "Admin");
            }
            AdminCreaateAndProfile adminCreaateAndProfile = _accessService.GetAdminCreaateAndProfile();
            model.Roles = adminCreaateAndProfile.Roles;
            model.Regions = adminCreaateAndProfile.Regions;
            _notyfService.Warning("Add Required Field.");
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
                int requestId = HttpContext.Session.GetInt32("requestId").Value;
                if (await _viewDocumentsServices.uploadFile(model,firstName:firstname,lastName: lastName,requestId))
                {
                    _notyfService.Success("Successfully File Added.");
                    return RedirectToAction("ViewDocument", "Admin");
                }
            }
            _notyfService.Warning("Please, Select File");
            return RedirectToAction("ViewDocument", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendOrder(SendOrder model)
        {
            if (ModelState.IsValid)
            {
                int requestId = HttpContext.Session.GetInt32("requestId").Value;
                if (await _sendOrderService.addOrderDetails(model,requestId))
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

        [HttpPost]
        public IActionResult ContactProvider(ContactProvider model)
        {
            if (ModelState.IsValid)
            {
                if (_providersService.contactProvider(model))
                {
                    _notyfService.Success("Successfully Message Send");
                }
                else
                {
                    _notyfService.Error("Message Send Faild");
                }
            }
            return RedirectToAction("Providers", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> CloseCase(CloseCase model)
        {
            if (ModelState.IsValid)
            {
                int requestId = HttpContext.Session.GetInt32("requestId").Value;
                if (await _closeCaseService.updateDetails(model,requestId))
                {
                    _notyfService.Success("Successfully Updated");
                }
                else
                {
                    _notyfService.Error("Update Faild");
                }
                return RedirectToAction("CloseCase", "Admin");
            }
            return View(model);
        }

        public async Task<IActionResult> RequestAddToCloseCase()    ///  from close case page button click
        {
            int requestId = HttpContext.Session.GetInt32("requestId").Value;
            if (await _closeCaseService.requestAddToCloseCase(requestId))
            {
                _notyfService.Success("Successfully Closed");
            }
            else
            {
                _notyfService.Error("Faild");
            }
            return RedirectToAction("Dashboard", "Admin");
        }

        [HttpGet]   // reset password through view profile
        public async Task<IActionResult> ViewProfileEditPassword(String newPassword)
        {
            int aspNetUserId = HttpContext.Session.GetInt32("aspNetUserId").Value;
            if (await _loginService.changePassword(aspNetUserId, newPassword))
            {
                _notyfService.Success("Successfully Updated");
            }
            else
            {
                _notyfService.Error("Faild");
            }
            return Json(new { redirect = Url.Action("ViewProfile", "Admin") });
        }

        [HttpGet]   //  Edit Administrator Information view profile
        public async Task<IActionResult> EditAdministratorInformation(String data1)
        {
            int aspNetUserId = HttpContext.Session.GetInt32("aspNetUserId").Value;
            if (await _viewProfileService.editEditAdministratorInformastion(data1,aspNetUserId))
            {
                _notyfService.Success("Successfully Updated");
            }
            else
            {
                _notyfService.Error("Faild");
            }
            return Json(new { redirect = Url.Action("ViewProfile", "Admin") });
        }

        [HttpGet]   //  Edit Mailing And Billing Information view profile
        public async Task<IActionResult> EditMailingAndBillingInformation(String data)
        {
            int aspNetUserId = HttpContext.Session.GetInt32("aspNetUserId").Value;
            if (await _viewProfileService.editMailingAndBillingInformation(data, aspNetUserId))
            {
                _notyfService.Success("Successfully Updated");
            }
            else
            {
                _notyfService.Error("Faild");
            }
            return Json(new { redirect = Url.Action("ViewProfile", "Admin") });
        }

        [HttpGet]   //  Edit Mailing And Billing Information view profile
        public async Task<IActionResult> EditProviderNotification(int physicanId,bool isNotification)
        {
            return Json(new { result =await _providersService.editProviderNotification(physicanId, isNotification)});    
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

        [HttpGet]   // Export selected Data 
        public IActionResult ExportData(String status, int pageNo, String partialViewName, String patinetName, int regionId, int requesterTypeId)
        {
            DataTable dataTable = _adminDashboardService.exportData(status, pageNo, patinetName, regionId, requesterTypeId);
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

        [HttpGet]   // Dashboard 
        public IActionResult GetTablesData(String status,int pageNo,String partialViewName,String patinetName, int regionId, int requesterTypeId)
        {
           TableModel tableModel= _adminDashboardService.GetNewRequest(status, pageNo, patinetName, regionId, requesterTypeId);
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

        [HttpGet]    // Send Order
        public HealthProfessional GetBussinessData(int venderId)
        {
            return _sendOrderService.getBussinessData(venderId);
        }
            
        [HttpGet]    // provider scheduling page 
        public IActionResult ChangeTab(string name,int regionId,int type,String time)
        {
            return PartialView(name, _providersService.getSchedulingTableDate(regionId,type,time));
        }
    }
}
