using Repositories.DataModels;
using Repositories.Interfaces;
using Repositories.ViewModels;
using Services.Interfaces.PatientServices;

namespace Services.Implementation.PatientServices
{
    public class PatientDashboardService : IPatientDashboardService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IRequestClientRepository _requestClientRepository;
        private readonly IRequestWiseFileRepository _requestWiseFileRepository;

        public PatientDashboardService(IUserRepository userRepository, IRequestRepository requestRepository,
            IRequestClientRepository requestClientRepository, IRequestWiseFileRepository requestWiseFileRepository)
        {
            _userRepository = userRepository;
            _requestRepository = requestRepository;
            _requestClientRepository = requestClientRepository;
            _requestWiseFileRepository = requestWiseFileRepository;
        }

        public List<Dashboard> GetUsersMedicalData(int aspNetUserId)
        {
            int userId = _userRepository.getUserID(aspNetUserId);
            List<RequestClient> requestClients = _requestClientRepository.getAllRequestClientForUser(userId);
            //
            //var usrRequests = (from req in requests
            //                join reqClient in requestClients
            //                on req.RequestId equals reqClient.RequestId
            //                where req.UserId == userId
            //                select new
            //                {
            //                    req,
            //                    reqClient
            //                }).ToList();
            //
            List<Dashboard> dashboards = new List<Dashboard>() { };
            DashboardHeader dashboardHeader = new()
            {
                PageType = 1,
                FirstName = requestClients[0].FirstName,
                LastName = requestClients[0].LastName,
                AspNetUserId = aspNetUserId,
            };
            foreach (RequestClient requestClient in requestClients)
            {
                Dashboard dashboard = new()
                {
                    RequestId = requestClient.RequestId,
                    StrMonth = requestClient.StrMonth,
                    Header = dashboardHeader,
                    IntYear = requestClient.IntYear,
                    IntDate = requestClient.IntDate,
                    Status = requestClient.Status,
                    Document = _requestWiseFileRepository.countFile(requestClient.RequestId),
                };
                dashboards.Add(dashboard);
            }
            return dashboards;
        }
    }
}
