using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Repositories.ViewModels.Admin;
using Services.Interfaces.Admin;

namespace HelloDoc.Controllers
{
    public class AdminController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IAdminDashboardService _adminDashboardService;
        private readonly IViewCaseService _viewCaseService;
        private readonly IViewNotesService _viewNotesService;

        public AdminController(INotyfService notyfService,IAdminDashboardService adminDashboardService, IViewCaseService viewCaseService,
                                IViewNotesService viewNotesService)
        {
            _notyfService = notyfService;
            _adminDashboardService = adminDashboardService;
            _viewCaseService = viewCaseService;
            _viewNotesService = viewNotesService;
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
                
                if (await _viewNotesService.addAdminTransform(model))
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
