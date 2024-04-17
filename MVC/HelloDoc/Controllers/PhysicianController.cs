using AspNetCoreHero.ToastNotification.Abstractions;
using HelloDoc.Authentication;
using Microsoft.AspNetCore.Mvc;
using Repositories.DataModels;
using Services.Interfaces.PhysicianServices;
using Services.ViewModels.Admin;

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

        public async Task<IActionResult> AcceptRequest(int requestId)
        {
            if (await _physicianDashboardService.acceptRequest(requestId))
            {
                _notyfService.Success("Successfully Accepted");
            }
            else
            {
                _notyfService.Error("Faild !!");
            }
            return RedirectToAction("Dashboard", "Physician");
        }

        [HttpPost]
        public async Task<IActionResult> TransferRequest(PhysicianTransferRequest model)
        {
            if (await _physicianDashboardService.transferRequest(model))
            {
                _notyfService.Success("Successfully Transfered");
            }
            else
            {
                _notyfService.Error("Faild !!");
            }
            return RedirectToAction("Dashboard", "Physician");
        }
            
        [HttpGet]
        public async Task<JsonResult> SetEncounter(bool isVideoCall,int requestId)
        {
            if(await _physicianDashboardService.setEncounter(requestId,isVideoCall))
            {
                HttpContext.Session.SetInt32("requestId", requestId);
                return Json(new { redirect = Url.Action("EncounterForm", "Admin") });
            }
            else
            {
                return Json(new { redirect = Url.Action("Dashboard", "Physician") });
            }
        }

        public IActionResult ViewProfile()
        {
            int aspNetUserId = HttpContext.Session.GetInt32("aspNetUserId").Value;
            int PhysicianId = _physicianDashboardService.getPhysicianIdFromAspNetUserId(aspNetUserId);
            return RedirectToAction("SetPhyscianId", "Admin", new { physicianId = PhysicianId });
        }

        public IActionResult FinalizeEncounter(EncounterForm model)
        {
            return RedirectToAction("SetPhyscianId", "Admin");
        }
    }
}
