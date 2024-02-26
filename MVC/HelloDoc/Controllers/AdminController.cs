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

        public AdminController(INotyfService notyfService,IAdminDashboardService adminDashboardService, IViewCaseService viewCaseService)
        {
            _notyfService = notyfService;
            _adminDashboardService = adminDashboardService;
            _viewCaseService = viewCaseService;
        }
        public IActionResult Dashboard()
        {
            return View(_adminDashboardService.getallRequests());
        }

        public IActionResult ViewCase(int id)
        {
            return View(_viewCaseService.getRequestDetails(requestId: id));
        }

        public IActionResult ViewNotes()
        {
            return View();
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
                return View(model);
            }
            return View(null);
        }

        public async Task<IActionResult> CancleRequest(int id)
        {
            await _viewCaseService.cancelRequest(id);
            return RedirectToAction("Dashboard","Admin");
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
    }
}
