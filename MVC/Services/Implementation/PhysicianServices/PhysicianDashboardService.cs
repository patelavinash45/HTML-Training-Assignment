using Repositories.DataModels;
using Repositories.Interfaces;
using Services.Interfaces.PhysicianServices;
using Services.ViewModels.Admin;
using Services.ViewModels.Physician;

namespace Services.Implementation.PhysicianServices
{
    public class PhysicianDashboardService : IPhysicianDashboardService
    {
        private readonly IRequestClientRepository _requestClientRepository;

        private Dictionary<string, List<int>> statusList { get; set; } = new Dictionary<string, List<int>>()
            {
                {"new", new List<int> { 1 } },
                {"pending", new List<int> { 2 } },
                {"active", new List<int> { 4, 5 } },
                {"conclude", new List<int> { 6 } },
            };

        public PhysicianDashboardService(IRequestClientRepository requestClientRepository)
        {
            _requestClientRepository = requestClientRepository;
        }

        public PhysicianDashboard getallRequests(int aspNetUserId)
        {
            CancelPopUp cancelPopUp = new()
            {
                Reasons = _requestClientRepository.getAllReason().ToDictionary(caseTag => caseTag.CaseTagId, caseTag => caseTag.Reason),
            };
            AssignAndTransferPopUp assignAndTransferPopUp = new()
            {
                Regions = _requestClientRepository.getAllRegions().ToDictionary(region => region.RegionId, region => region.Name),
            };
            return new PhysicianDashboard()
            {
                NewRequests = GetNewRequest(status: "new", pageNo: 1, patientName: "", regionId: 0, requesterTypeId: 0, aspNetUserId),
                NewRequestCount = _requestClientRepository.countRequestClientByStatusForPhysician(statusList["new"],aspNetUserId),
                PendingRequestCount = _requestClientRepository.countRequestClientByStatusForPhysician(statusList["pending"], aspNetUserId),
                ActiveRequestCount = _requestClientRepository.countRequestClientByStatusForPhysician(statusList["active"], aspNetUserId),
                ConcludeRequestCount = _requestClientRepository.countRequestClientByStatusForPhysician(statusList["conclude"], aspNetUserId),
                CancelPopup = cancelPopUp,
                AssignAndTransferPopup = assignAndTransferPopUp,
            };
        }

        public TableModel GetNewRequest(String status, int pageNo, String patientName, int regionId, int requesterTypeId, int aspNetUserId)
        {
            int skip = (pageNo - 1) * 10;
            Func<RequestClient, bool> predicate = a =>
            (requesterTypeId == 0 || a.Request.RequestTypeId == requesterTypeId)
            && (regionId == 0 || a.RegionId == regionId)
            && (patientName == null || a.FirstName.ToLower().Contains(patientName) || a.LastName.ToLower().Contains(patientName))
            && (!statusList[status].Contains(1) || a.Physician != null)
            && a.Physician != null
            && a.Physician.AspNetUserId == aspNetUserId
            && (statusList[status].Contains(a.Status));
            int totalRequests = _requestClientRepository.countRequestClientByStatusAndFilter(predicate);
            List<RequestClient> requestClients = _requestClientRepository.getRequestClientByStatus(predicate, skip: skip);
            return getTableModal(requestClients, totalRequests, pageNo);
        }

        private TableModel getTableModal(List<RequestClient> requestClients, int totalRequests, int pageNo)
        {
            int skip = (pageNo - 1) * 10;
            List<TablesData> tablesDatas = requestClients
                .Select(requestClient => new TablesData()
                {
                    RequestId = requestClient.RequestId,
                    FirstName = requestClient.FirstName,
                    LastName = requestClient.LastName,
                    Requester = requestClient.Request.RequestTypeId,
                    RequesterFirstName = requestClient.Request.FirstName,
                    RequesterLastName = requestClient.Request.LastName,
                    Mobile = requestClient.PhoneNumber,
                    RequesterMobile = requestClient.Request.PhoneNumber,
                    State = requestClient.State,
                    Street = requestClient.Street,
                    ZipCode = requestClient.ZipCode,
                    City = requestClient.City,
                    Notes = requestClient.Symptoms,
                    RequesterType = requestClient.Request.RequestTypeId,
                    Email = requestClient.Email,
                }).ToList();
            int totalPages = totalRequests % 10 != 0 ? (totalRequests / 10) + 1 : totalRequests / 10;
            return new TableModel()
            {
                IsFirstPage = pageNo != 1,
                IsLastPage = pageNo != totalPages,
                IsNextPage = pageNo < totalPages,
                IsPreviousPage = pageNo > 1,
                TableDatas = tablesDatas,
                TotalRequests = totalRequests,
                PageNo = pageNo,
                StartRange = skip + 1,
                EndRange = skip + 10 < totalRequests ? skip + 10 : totalRequests,
            };
        }
    }
}
