using Repositories.DataModels;
using Repositories.Interfaces;
using Repositories.ViewModels;
using Services.Interfaces;
using System.Collections;
using System.Net.Mail;
using System.Net;

namespace Services.Implementation
{
    public class ViewDocumentsServices : IViewDocumentsServices
    {
        private readonly IRequestWiseFileRepository _requestWiseFileRepository;
        private readonly IRequestClientRepository _requestClientRepository;
        private readonly IFileService _fileService;
        private readonly IAdminRepository _adminRepository;

        public ViewDocumentsServices(IRequestWiseFileRepository requestWiseFileRepository, IRequestClientRepository requestClientRepository,
                                      IFileService fileService, IAdminRepository adminRepository)
        {
            _requestWiseFileRepository = requestWiseFileRepository;
            _requestClientRepository = requestClientRepository;
            _fileService = fileService;
            _adminRepository = adminRepository;
        }

        public ViewDocument getDocumentList(int requestId, int aspNetUserId)
        {
            List<RequestWiseFile> requestWiseFiles = _requestWiseFileRepository.getFilesByrequestId(requestId);
            RequestClient requestClient = _requestClientRepository.GetRequestClientByRequestId(requestId);
            Admin admin = _adminRepository.getAdmionByAspNetUserId(aspNetUserId);
            DashboardHeader dashboardHeader = new()
            {
                FirstName = admin!=null?admin.FirstName:requestClient.FirstName, 
                LastName = admin != null ? admin.LastName:requestClient.LastName,
                AspNetUserId = aspNetUserId,
            };
            ViewDocument viewDocument = new()
            {
                FirstName = requestClient.FirstName,
                LastName = requestClient.LastName,
                FileList = requestWiseFiles,
                Header = dashboardHeader,
                RequestId = requestClient.RequestId,
            };
            return viewDocument;
        }

        public async Task<int> uploadFile(ViewDocument model)
        {
            return await _fileService.addFile(requestId: model.RequestId, firstName: model.Header.FirstName, lastName: model.Header.LastName,
                                                      file: model.File);
        }

        public async Task<int> deleteFile(int requestWiseFileId)
        {
            RequestWiseFile requestWiseFile = _requestWiseFileRepository.getFilesByrequestWiseFileId(requestWiseFileId);
            requestWiseFile.IsDeleted = new BitArray(1, true);
            if(_fileService.removeFile(requestWiseFile.FileName))
            {
                return await _requestWiseFileRepository.updateRequestWiseFile(requestWiseFile) ? requestWiseFile.RequestId : 0;
            }
            return 0;
        }

        public async Task<int> deleteAllFile(List<int> requestWiseFileIds)
        {
            int requestId = 0;
            foreach(var item in requestWiseFileIds)
            {
                requestId = await deleteFile(item);
            }
            return requestId;
        }

        public async Task<int> sendFileMail(List<int> requestWiseFileIds)
        {
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("tatva.dotnet.avinashpatel@outlook.com"),
                Subject = "Reset Password Link",
                IsBodyHtml = true,
            };
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
            RequestWiseFile requestWiseFile = new RequestWiseFile();
            foreach (var item in requestWiseFileIds)
            {
                requestWiseFile = _requestWiseFileRepository.getFilesByrequestWiseFileId(item);
                path = Path.Combine(path, requestWiseFile.FileName);
                Attachment attachment = new Attachment(path);
                mailMessage.Attachments.Add(attachment);
            }
            RequestClient requestClient = _requestClientRepository.GetRequestClientByRequestId(requestWiseFile.RequestId);
            mailMessage.Body = "All The Documents For RequestId : " + requestWiseFile.RequestId.ToString();
            //mailMessage.To.Add(requestClient.Email);
            mailMessage.To.Add("tatva.dotnet.avinashpatel@outlook.com");
            SmtpClient smtpClient = new SmtpClient("smtp.office365.com")
            {
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                Port = 587,
                Credentials = new NetworkCredential(userName: "tatva.dotnet.avinashpatel@outlook.com", password: "Avinash@6351"),
            };
            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                return requestWiseFile.RequestId;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
