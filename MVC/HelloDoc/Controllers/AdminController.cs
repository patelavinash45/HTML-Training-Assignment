using Azure;
using HelloDoc.DataContext;
using HelloDoc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Repositories.ViewModels.Admin;
using Services.Interfaces;
using Services.Interfaces.Admin;

namespace HelloDoc.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminDashboardService _adminDashboardService;
        private readonly IViewCaseService _viewCaseService;

        public AdminController(IAdminDashboardService adminDashboardService, IViewCaseService viewCaseService)
        {
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
