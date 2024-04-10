﻿using Repositories.DataModels;
using Repositories.Interfaces;
using Services.Interfaces.AdminServices;
using Services.ViewModels.Admin;
using System.Data;
using System.Reflection;
using System.Text.Json;

namespace Services.Implementation.AdminServices
{
    public class RecordService : IRecordService
    {
        private readonly IRequestClientRepository _requestClientRepository;
        private readonly ILogsService _logsService;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public RecordService(IRequestClientRepository requestClientRepository,ILogsService logsService, IRoleRepository roleRepository,
                                       IUserRepository userRepository)
        {
            _requestClientRepository = requestClientRepository;
            _logsService = logsService;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        public Records getRecords(Records model)
        {
            Func<RequestClient, bool> predicat = a =>
            (model.RequestType == null || model.RequestType == a.Request.RequestTypeId)
            && (model.Email == null || a.Email.ToLower().Contains(model.Email.ToLower()))
            && (model.Number == null || a.PhoneNumber.Contains(model.Number))
            && (model.StartDate == null || a.Request.AcceptedDate >= model.StartDate)
            && (model.EndDate == null || a.Request.AcceptedDate <= model.EndDate)
            && (model.PatientName == null || a.FirstName.ToLower().Contains(model.PatientName.ToLower())
                                          || a.LastName.ToLower().Contains(model.PatientName.ToLower())
                                          || $"{a.FirstName} {a.LastName}".ToLower().Contains(model.PatientName.ToLower()))
            && (model.ProviderName == null || ( a.Physician != null && (a.Physician.FirstName.ToLower().Contains(model.ProviderName.ToLower())
                                                                     || a.Physician.LastName.ToLower().Contains(model.ProviderName.ToLower()))))
            && (model.RequestStatus == null || a.Status == model.RequestStatus)
            && (a.Status == 3 || a.Status == 7 || a.Status == 8);
            model.RecordTableDatas = _requestClientRepository.getRequestClientsBasedOnFilter(predicat)
                .Select(requestClient =>
                {
                    RequestNote requestNote = requestClient.Request.RequestNotes.FirstOrDefault(a => a.RequestId == requestClient.RequestId);
                    return new RecordTableData()
                    {
                        RequestId = requestClient.RequestId,
                        PatientName = $"{requestClient.FirstName} {requestClient.LastName}",
                        PhysicianName = requestClient.Physician != null ? $"{requestClient.Physician.FirstName} {requestClient.Physician.LastName}" : "-",
                        Address = $"{requestClient.Street},{requestClient.City},{requestClient.State},{requestClient.ZipCode}",
                        RequestType = requestClient.Request.RequestTypeId,
                        DateOfService = requestClient.Request.AcceptedDate,
                        Email = requestClient.Email,
                        Phone = requestClient.PhoneNumber,
                        Zip = requestClient.ZipCode,
                        Status = requestClient.Status,
                        PhysicianNotes = requestNote != null ? requestNote.PhysicianNotes : "-",
                        AdminNotes = requestNote != null ? requestNote.AdminNotes : "-",
                    };
                }).ToList();
            return model;
        }

        public EmailSmsLogs getEmailLog(EmailSmsLogs model)
        {
            model.EmailSmsLogTableDatas = getEmailLogTabledata(model);
            model.Roles = _roleRepository.getAllRoles().ToDictionary(role => role.RoleId, role => role.Name);
            return model;
        }

        public List<EmailSmsLogTableData> getEmailLogTabledata(EmailSmsLogs model)
        {
            Func<EmailLog, bool> predicat = a =>
            (model.Email == null || a.EmailId.ToLower().Contains(model.Email))
            && (model.Name == null || a.Name.Contains(model.Name))
            && (model.Role == null || a.RoleId == model.Role)
            && (model.CreatedDate == null || DateOnly.FromDateTime(a.CreateDate) == model.CreatedDate)
            && (model.SendDate == null || DateOnly.FromDateTime(a.SentDate) == model.SendDate);
            return _logsService.getAllEmailLogs(predicat).Select(emailLog => new EmailSmsLogTableData()
            {
                Name = emailLog.Name,
                Action = emailLog.SubjectName,
                CreatedDate = emailLog.CreateDate,
                SendDate = emailLog.SentDate,
                Send = emailLog.IsEmailSent[0] ? "Yes" : "No",
                Email = emailLog.EmailId,
                RoleName = emailLog.Role.Name,
            }).ToList();
        }

        public EmailSmsLogs getSMSlLog(EmailSmsLogs model)
        {
            model.EmailSmsLogTableDatas = getSMSLogTabledata(model);
            model.Roles = _roleRepository.getAllRoles().ToDictionary(role => role.RoleId, role => role.Name);
            return model;
        }

        public List<EmailSmsLogTableData> getSMSLogTabledata(EmailSmsLogs model)
        {
            Func<Smslog, bool> predicat = a =>
            (model.Phone == null || a.MobileNumber.Contains(model.Phone))
            && (model.Name == null || a.Name.Contains(model.Name))
            && (model.Role == null || a.RoleId == model.Role)
            && (model.CreatedDate == null || DateOnly.FromDateTime(a.CreateDate) == model.CreatedDate)
            && (model.SendDate == null || DateOnly.FromDateTime(a.SentDate) == model.SendDate);
            return _logsService.getAllSMSLogs(predicat).Select(emailLog => new EmailSmsLogTableData()
            {
                Name = emailLog.Name,
                Action = emailLog.Smstemplate,
                CreatedDate = emailLog.CreateDate,
                SendDate = emailLog.SentDate,
                Send = emailLog.IsSmssent[0] ? "Yes" : "No",
                RoleName = emailLog.Role.Name,
                Phone = emailLog.MobileNumber,
            }).ToList();
        }

        public DataTable ExportAllRecords()
        {
            List<String> columnsNames = new List<String>();
            DataTable dataTable = new DataTable();
            dataTable.TableName = "Records";
            int currentRow = 1, index = 1;
            foreach (PropertyInfo propertyInfo in typeof(RequestClient).GetProperties())
            {
                dataTable.Columns.Add(propertyInfo.Name);
                columnsNames.Add(propertyInfo.Name);
                index++;
            }
            Func<RequestClient, bool> predicat = a => (a.Status == 3 || a.Status == 7 || a.Status == 8);
            DataRow row;
            foreach (RequestClient requestClient in _requestClientRepository.getRequestClientsBasedOnFilter(predicat))
            {
                row = dataTable.NewRow();
                for (int i = 0; i < columnsNames.Count; i++)
                {
                    var value = typeof(RequestClient).GetProperty(columnsNames[i]).GetValue(requestClient);
                    if (value != null)
                    {
                        row[columnsNames[i]] = value.ToString();
                    }
                }
                dataTable.Rows.Add(row);
                currentRow++;
            }
            return dataTable;
        }

        public PatientHistory getPatientHistory(PatientHistory model,int pageNo)
        {
            int skip = (pageNo - 1) * 5;
            Func<User, bool> predicat = a =>
            (model.Email == null || a.Email.Contains(model.Email))
            && (model.Phone == null || a.Mobile.Contains(model.Phone))
            && (model.FirstName == null || a.FirstName.ToLower().Contains(model.FirstName.ToLower()))
            && (model.LastName == null || a.LastName.ToLower().Contains(model.LastName.ToLower()));
            int totalPatient = _userRepository.countUsers(predicat);
            List<PatientHistoryTableData> patientHistoryTableDatas = _userRepository.getAllUser(predicat,skip)
                    .Select(user => new PatientHistoryTableData()
                    {
                        UserId = user.UserId, 
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Phone = user.Mobile,
                        Address = $"{user.House}, {user.Street}, {user.City}, {user.State}, {user.ZipCode}"
                    }).ToList();
            int totalPages = totalPatient % 5 != 0 ? (totalPatient / 5) + 1 : totalPatient / 5;
            model.PatientHistoryTable = new PatientHistoryTable()
            {
                IsFirstPage = pageNo != 1,
                IsLastPage = pageNo != totalPages,
                IsNextPage = pageNo < totalPages,
                IsPreviousPage = pageNo > 1,
                TotalRequests = totalPatient,
                PageNo = pageNo,
                StartRange = skip + 1,
                EndRange = skip + 5 < totalPatient ? skip + 5 : totalPatient,
                PatientHistoryTableDatas = patientHistoryTableDatas,
            };
            return model;
        }

        public PatientHistory getSMSLogTabledata(string data, int pageNo)
        {
            PatientHistory model = JsonSerializer.Deserialize<PatientHistory>(data);
            return getPatientHistory(model, pageNo);
        }
    }
}
