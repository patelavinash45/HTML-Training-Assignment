//using HelloDoc.Authentication;
//using Microsoft.AspNetCore.Mvc;
//using Services.Implementation.AdminServices;
//using Services.Interfaces.AdminServices;

//namespace HelloDoc.Controllers
//{
//    public class BaseController : Controller
//    {
//        private readonly IAdminDashboardService _adminDashboardService;
//        private readonly IViewNotesService _viewNotesService;

//        public BaseController(IAdminDashboardService adminDashboardService, IViewNotesService viewNotesService)
//        {
//            _adminDashboardService = adminDashboardService;
//            _viewNotesService = viewNotesService;
//        }

//        [HttpGet]  /// set requestId in session
//        public JsonResult SetRequestId(int requestId, String actionName)
//        {
//            HttpContext.Session.SetInt32("requestId", requestId);
//            return Json(new { redirect = Url.Action(actionName) });
//        }

//        //[Authorization("Admin")]
//        [HttpGet("/Physician/ViewCase", Name = "Physician")]
//        [HttpGet("/Admin/ViewCase", Name = "Admin")]
//        public IActionResult ViewCase()
//        {
//            int requestId = HttpContext.Session.GetInt32("requestId").Value;
//            return View(_adminDashboardService.getRequestDetails(requestId));
//        }

//        //[Authorization("Admin")]
//        [HttpGet("/Physician/ViewNotes", Name = "Physician")]
//        [HttpGet("/Admin/ViewNotes", Name = "Admin")]
//        public IActionResult ViewNotes()
//        {
//            int requestId = HttpContext.Session.GetInt32("requestId").Value;
//            return View(_viewNotesService.GetNotes(requestId));
//        }
//    }
//}
