using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Repositories.ViewModels;
using Repositories.ViewModels.Admin;
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

        public AdminController(INotyfService notyfService,IAdminDashboardService adminDashboardService, IViewCaseService viewCaseService,
                                IViewNotesService viewNotesService, ILoginService loginService,IViewDocumentsServices viewDocumentsServices)
        {
            _notyfService = notyfService;
            _loginService = loginService;
            _adminDashboardService = adminDashboardService;
            _viewCaseService = viewCaseService;
            _viewNotesService = viewNotesService;
            _viewDocumentsServices = viewDocumentsServices;
        }
        public IActionResult LoginPage()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("aspNetUserId");
            return RedirectToAction("LoginPage", "Admin");
        }

        public IActionResult Dashboard()
        {
            int aspNetUseId = HttpContext.Session.GetInt32("aspNetUserId").Value;
            return aspNetUseId > 0 ? View(_adminDashboardService.getallRequests(aspNetUseId)) : View(null);
        }

        public IActionResult ViewCase(int requestId)
        {
            return View(_viewCaseService.getRequestDetails(requestId));
        }

        public IActionResult ViewNotes(int requestId)
        {
            return View(_viewNotesService.GetNotes(requestId));
        }

        public IActionResult ViewDocument(int requestId)
        {
            int aspNetUserId = HttpContext.Session.GetInt32("aspNetUserId").Value;
            return View(_viewDocumentsServices.getDocumentList(requestId: requestId, aspNetUserId: aspNetUserId));
        }

        public async Task<JsonResult> DeleteAll([FromBody]List<int> requestWiseFileIdsList)
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

        public async Task<IActionResult> DeleteFiles(int requestWiseFileId)
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
                if (await _viewDocumentsServices.uploadFile(model) > 0)
                {
                    return RedirectToAction("ViewDocument", "Admin", new { requestId = model.RequestId });
                }
            }
            _notyfService.Warning("Please, Add Required Field.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginPage(Login model)
        {
            if (ModelState.IsValid)
            {
                int aspNetUserId = _loginService.auth(model,2);
                if (aspNetUserId == 0)
                {
                    _notyfService.Error("Invalid credentials");
                }
                else
                {
                    HttpContext.Session.SetInt32("aspNetUserId", aspNetUserId);
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
    }
}
