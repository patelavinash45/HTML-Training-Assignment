using Repositories.ViewModels;
using Microsoft.EntityFrameworkCore;
using Repositories.DataModels;
using Repositories.Interface;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.ViewModels;
using HelloDoc.ViewModels;

namespace Services.Implementation
{
    public class DashboardService : IDashboardService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IRequestClientRepository _requestClientRepository;
        private readonly IRequestWiseFileRepository _RequestWiseFileRepository;

        public DashboardService(IUserRepository userRepository, IRequestRepository requestRepository, 
            IRequestClientRepository requestClientRepository, IRequestWiseFileRepository requestWiseFileRepository)
        {
            _userRepository = userRepository;
            _requestRepository = requestRepository;
            _requestClientRepository = requestClientRepository;
            _RequestWiseFileRepository = requestWiseFileRepository;
        }

        public List<Dashboard> GetUsersMedicalData(int aspNetUserId)
        {
            int userId = _userRepository.getUserID(aspNetUserId);
            List<Request> requests= _requestRepository.getAllRequest();
            List<RequestClient> requestClients = _requestClientRepository.getAllRequestClient();
            var usrRequests = (from req in requests
                            join reqClient in requestClients
                            on req.RequestId equals reqClient.RequestId
                            where req.UserId == userId
                            select new
                            {
                                req,
                                reqClient
                            }).ToList();
            List<Dashboard> dashboards = new List<Dashboard>() { };
            DashboardHeader dashboardHeader = new()
            {
                FirstName = usrRequests[0].reqClient.FirstName,
                LastName = usrRequests[0].reqClient.LastName,
                AspNetUserId = aspNetUserId,
            };
            Dashboard dashboard = new()
            {
                RequestId = usrRequests[0].req.RequestId,
                StrMonth = usrRequests[0].reqClient.StrMonth,
                Header = dashboardHeader,
                IntYear = usrRequests[0].reqClient.IntYear,
                IntDate = usrRequests[0].reqClient.IntDate,
                Status = usrRequests[0].reqClient.Status,
                Document = _RequestWiseFileRepository.countFile(usrRequests[0].req.RequestId),
            };
            dashboards.Add(dashboard);
            for(int i=1;i< usrRequests.Count;i++)
            {
                dashboard = new()
                {
                    RequestId = usrRequests[i].req.RequestId,
                    StrMonth = usrRequests[i].reqClient.StrMonth,
                    Header = dashboardHeader,
                    IntYear = usrRequests[i].reqClient.IntYear,
                    IntDate = usrRequests[i].reqClient.IntDate,
                    Status = usrRequests[i].reqClient.Status,
                    Document = _RequestWiseFileRepository.countFile(usrRequests[i].req.RequestId),
                };
                dashboards.Add(dashboard);
            }
            return dashboards;
        }
    }
}
