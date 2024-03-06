﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.DataModels;
using Repositories.Interfaces;
using Repositories.ViewModels;
using Repositories.ViewModels.Admin;
using Services.Interfaces.AdminServices;

namespace Services.Implementation.AdminServices
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IRequestClientRepository _requestClientRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly ICaseTagRepository _caseTagRepository;
        private readonly IPhysicianRepository _physicianRepository;

        public AdminDashboardService(IRegionRepository regionRepository,IRequestClientRepository requestClientRepository,
                                       ICaseTagRepository caseTagRepository, IPhysicianRepository physicianRepository)
        { 
            _regionRepository = regionRepository;
            _requestClientRepository = requestClientRepository;
            _caseTagRepository = caseTagRepository;
            _physicianRepository = physicianRepository;
        }

        public AdminDashboard getallRequests(int aspNetUserId)
        {
            List<Region> allRegion = _regionRepository.getAllRegions();
            CancelPopUp cancelPopUp = new()
            {
                Reasons= _caseTagRepository.getAllReason(),
            };
            AssignPopUp assignPopUp = new()
            {
                Regions = allRegion,
                Physics = _physicianRepository.getAllPhysicians(),
            };
            DashboardHeader dashboardHeader = new()
            {
                PageType = 1,
            };
            AdminDashboard adminDashboard = new()
            {
                NewRequests = GetNewRequest(status:"New",pageNo:1),
                NewRequestCount = _requestClientRepository.countRequestClientByStatus(1),
                PendingRequestCount = _requestClientRepository.countRequestClientByStatus(2),
                ActiveRequestCount = _requestClientRepository.countRequestClientByStatus(4)  +
                                     _requestClientRepository.countRequestClientByStatus(5),
                ConcludeRequestCount = _requestClientRepository.countRequestClientByStatus(6),
                TocloseRequestCount = _requestClientRepository.countRequestClientByStatus(3) +
                                      _requestClientRepository.countRequestClientByStatus(7) +
                                      _requestClientRepository.countRequestClientByStatus(8),
                UnpaidRequestCount = _requestClientRepository.countRequestClientByStatus(9),
                Regions = allRegion,
                Header = dashboardHeader,
                CancelPopup = cancelPopUp,
                AssignPopup = assignPopUp,
            };
            return adminDashboard;
        }

        public TableModel GetNewRequest(String status,int pageNo)
        {
            int skip = (pageNo - 1) * 10;
            int totalRequests = 0;
            List<RequestClient> requestClients= new List<RequestClient>();
            if (status=="New")
            {
                totalRequests = _requestClientRepository.countRequestClientByStatus(1);
                requestClients = _requestClientRepository.getRequestClientByStatus(status:1,skip:skip);
            }
            else if(status == "Pending")
            {
                totalRequests = _requestClientRepository.countRequestClientByStatus(2);
                requestClients = _requestClientRepository.getRequestClientByStatus(status: 2, skip: skip);
            }
            else if (status == "Active")
            {
                totalRequests = _requestClientRepository.countRequestClientByStatus(4)+ _requestClientRepository.countRequestClientByStatus(5);
                requestClients = _requestClientRepository.getRequestClientByStatus(status: 4, skip: skip);
                requestClients.AddRange(_requestClientRepository.getRequestClientByStatus(status: 5, skip: skip));
            }
            else if (status == "Conclude")
            {
                totalRequests = _requestClientRepository.countRequestClientByStatus(6);
                requestClients = _requestClientRepository.getRequestClientByStatus(status: 6, skip: skip);
            }
            else if (status == "Close")
            {
                totalRequests = _requestClientRepository.countRequestClientByStatus(3) + _requestClientRepository.countRequestClientByStatus(7)
                                               + _requestClientRepository.countRequestClientByStatus(8);
                requestClients = _requestClientRepository.getRequestClientByStatus(status: 3, skip: skip);
                requestClients.AddRange(_requestClientRepository.getRequestClientByStatus(status: 7, skip: skip));
                requestClients.AddRange(_requestClientRepository.getRequestClientByStatus(status: 8, skip: skip));
            }
            else if (status == "Unpaid")
            {
                totalRequests = _requestClientRepository.countRequestClientByStatus(9);
                requestClients = _requestClientRepository.getRequestClientByStatus(status: 9, skip: skip);
            }
            //
            //var allnewRequests = (from req in requests
            //                      join reqClient in requestClients
            //                      on req.RequestId equals reqClient.RequestId
            //                      where reqClient.Status == 1 
            //                      select new
            //                      {
            //                          req,
            //                          reqClient
            //                      }).ToList();
            //
            List<TablesData> tablesDatas = new List<TablesData>();
            foreach(RequestClient requestClient in requestClients)
            {
                TablesData tablesData = new()
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
                    RegionId = requestClient.RegionId,
                    RequesterType = requestClient.Request.RequestTypeId,
                    BirthDate = requestClient.IntYear!=null? DateTime.Parse(requestClient.IntYear + "-" + requestClient.StrMonth 
                                 + "-" + requestClient.IntDate): null,
                    RequestdDate = requestClient.Request.CreatedDate != null ? requestClient.Request.CreatedDate : null,
                    Email = requestClient.Email,
                    DateOfService = null,
                    PhysicianName = "",
                };
                tablesDatas.Add(tablesData);
            }
            int totalPages = (totalRequests / 10) + 1;
            TableModel tableModel = new()
            {
                IsNextPage = pageNo < totalPages,
                IsPreviousPage = pageNo > 1,
                TableDatas = tablesDatas,
                TotalRequests = totalRequests,
                PageNo = pageNo,
                StartRange = skip+1,
                EndRange = skip+10 < totalRequests ? skip+10 : totalRequests,
            };
            return tableModel;
        }

        public TableModel searchPatient(String patientName)
        {
            String[] names = patientName.Split(" ");
            List<RequestClient> requestClients = _requestClientRepository.getRequestClientByName(firstName: names[0], lastName: names[1]);
            List<TablesData> tablesDatas = new List<TablesData>();
            foreach (RequestClient requestClient in requestClients)
            {
                TablesData tablesData = new()
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
                    RegionId = requestClient.RegionId,
                    RequesterType = requestClient.Request.RequestTypeId,
                    BirthDate = requestClient.IntYear != null ? DateTime.Parse(requestClient.IntYear + "-" + requestClient.StrMonth
                                 + "-" + requestClient.IntDate) : null,
                    RequestdDate = requestClient.Request.CreatedDate != null ? requestClient.Request.CreatedDate : null,
                    Email = requestClient.Email,
                    DateOfService = null,
                    PhysicianName = "",
                };
                tablesDatas.Add(tablesData);
            }
            int totalPages = (requestClients.Count / 10) + 1;
            TableModel tableModel = new()
            {
                IsNextPage = 1 < totalPages,
                IsPreviousPage = false,
                TableDatas = tablesDatas,
                TotalRequests = requestClients.Count,
                PageNo = 1,
                StartRange = 1,
                EndRange = 10 < requestClients.Count ? 10 : requestClients.Count,
            };
            return tableModel;
        }
    }
}
