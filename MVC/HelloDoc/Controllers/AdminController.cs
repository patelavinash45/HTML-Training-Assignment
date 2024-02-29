using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Repositories.ViewModels;
using Repositories.ViewModels.Admin;
using Services.Interfaces.Admin;
using Services.Interfaces.Auth;

namespace HelloDoc.Controllers
{
    public class AdminController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly ILoginService _loginService;
        private readonly IAdminDashboardService _adminDashboardService;
        private readonly IViewCaseService _viewCaseService;
        private readonly IViewNotesService _viewNotesService;

        public AdminController(INotyfService notyfService,IAdminDashboardService adminDashboardService, IViewCaseService viewCaseService,
                                IViewNotesService viewNotesService, ILoginService loginService)
        {
            _notyfService = notyfService;
            _loginService = loginService;
            _adminDashboardService = adminDashboardService;
            _viewCaseService = viewCaseService;
            _viewNotesService = viewNotesService;
        }
        public IActionResult LoginPage()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View(_adminDashboardService.getallRequests());
        }

        public IActionResult ViewCase(int requestId)
        {
            return View(_viewCaseService.getRequestDetails(requestId));
        }

        public async Task<IActionResult> ViewNotes(int requestId)
        {
            return View(await _viewNotesService.GetNotes(requestId));
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
                    return View(null);
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
            return View(null);
        }

        [HttpGet]
        public IActionResult GetTablesData(String status)
        {
           List<NewTables> tableData= _adminDashboardService.GetNewRequest(status);
           switch(status)
            {
                case "New": return PartialView("_NewTable", tableData); 
                case "Pending": return PartialView("_PendingTable", tableData);
                case "Active": return PartialView("_ActiveTable", tableData);
                case "Conclude": return PartialView("_ConcludeTable", tableData);
                case "Close": return PartialView("_CloseTable", tableData);
                case "Unpaid": return PartialView("_UnpaidTable", tableData);
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
