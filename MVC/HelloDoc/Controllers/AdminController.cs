using HelloDoc.DataContext;
using HelloDoc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Interfaces.Admin;

namespace HelloDoc.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminDashboardService _adminDashboardService;

        public AdminController(IAdminDashboardService adminDashboardService)
        {
            _adminDashboardService = adminDashboardService;
        }
        public IActionResult Dashboard()
        {
            return View(_adminDashboardService.getallRequests());
        }

        //[HttpGet]
        //public IActionResult GetTablesData(string status)
        //{
        //    PendingTable pendingtable = new()
        //    {
        //        status = status,
        //    };
        //    return PartialView("_PendingTable",pendingtable);
        //}
    }
}
