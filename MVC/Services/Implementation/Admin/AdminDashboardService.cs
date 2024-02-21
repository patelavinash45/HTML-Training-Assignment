using Repositories.DataModels;
using Repositories.Implementation;
using Repositories.Interface;
using Repositories.Interfaces;
using Repositories.ViewModels.Admin;
using Services.Interfaces;
using Services.Interfaces.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation.Admin
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IRequestClientRepository _requestClientRepository;

        public AdminDashboardService(IRequestRepository requestRepository,
                                    IRequestClientRepository requestClientRepository)
        { 
            _requestRepository = requestRepository;
            _requestClientRepository = requestClientRepository;
        }

        public AdminDashboard getallRequests()
        {
            List<Request> requests = _requestRepository.getAllRequest();
            List<RequestClient> requestClients = _requestClientRepository.getAllRequestClient();
            var allnewRequests = (from req in requests
                               join reqClient in requestClients
                               on req.RequestId equals reqClient.RequestId
                               where reqClient.Status==1
                                  select new
                               {
                                   req,
                                   reqClient
                               }).ToList();
            List<NewTables> newTables = new List<NewTables>();
            for(int i = 0; i < allnewRequests.Count; i++)
            {
                NewTables newTable = new()
                {
                    FirstName = allnewRequests[i].reqClient.FirstName,
                    LastName = allnewRequests[i].reqClient.LastName,
                    Requesters = allnewRequests[i].req.FirstName + allnewRequests[i].req.LastName,
                    Mobile = allnewRequests[i].reqClient.PhoneNumber,
                    State = allnewRequests[i].reqClient.State,
                    Street = allnewRequests[i].reqClient.Street,
                    ZipCode = allnewRequests[i].reqClient.ZipCode,
                    City = allnewRequests[i].reqClient.City,
                };
                newTables.Add(newTable);
            }
            AdminDashboard adminDashboard = new()
            {
                NewRequests = newTables,
            };
            return adminDashboard;
        }
    }
}
