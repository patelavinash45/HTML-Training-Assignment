using AspNetCoreHero.ToastNotification.Abstractions;
using HelloDoc.Authentication;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces.PhysicianServices;
using Services.ViewModels.Admin;
using Services.ViewModels.Physician;

namespace HelloDoc.Controllers
{
    public class PhysicianController : Controller
    {

        private readonly INotyfService _notyfService;
        private readonly IPhysicianDashboardService _physicianDashboardService;

        public PhysicianController(INotyfService notyfService, IPhysicianDashboardService physicianDashboardService)
        {
            _notyfService = notyfService;
            _physicianDashboardService = physicianDashboardService;
        }

        [Authorization("Physician")]
        public IActionResult Dashboard()
        {
            int aspNetUserId = HttpContext.Session.GetInt32("aspNetUserId").Value;
            return View(_physicianDashboardService.getallRequests(aspNetUserId));
        }

        [HttpGet]   // Dashboard 
        public IActionResult GetTablesData(String status, int pageNo, String partialViewName, String patinetName, int regionId, int requesterTypeId)
        {
            int aspNetUserId = HttpContext.Session.GetInt32("aspNetUserId").Value;
            TableModel tableModel = _physicianDashboardService.GetNewRequest(status, pageNo, patinetName, regionId, requesterTypeId, aspNetUserId);
            return tableModel.TableDatas.Count != 0 ? PartialView(partialViewName, tableModel) : PartialView("_NoTableDataFound");
        }
    }
}
