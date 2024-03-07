using Repositories.DataModels;
using Repositories.Interfaces;
using Services.Interfaces;
using Services.ViewModels;
using System.Collections;
using System.Net;
using System.Net.Mail;

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
            List<FileModel> fileList = new List<FileModel>();
            foreach (var file in requestWiseFiles)
            {
                FileModel fileModel = new FileModel()
                {
                    RequestId = requestId,
                    RequestWiseFileId = file.RequestWiseFileId,
                    FileName = file.FileName,
                    Uploder = file.Uploder,
                    CreatedDate = file.CreatedDate,
                };
                fileList.Add(fileModel);
            }
            RequestClient requestClient = _requestClientRepository.GetRequestClientByRequestId(requestId);
            Admin admin = _adminRepository.getAdmionByAspNetUserId(aspNetUserId);
            ViewDocument viewDocument = new()
            {
                FirstName = requestClient.FirstName,
                LastName = requestClient.LastName,
                FileList = fileList,
                RequestId = requestClient.RequestId,
            };
            return viewDocument;
        }

        public async Task<int> uploadFile(ViewDocument model ,String firstName,String lastName)
        {
            return await _fileService.addFile(requestId: model.RequestId, firstName: firstName, lastName: lastName,file: model.File);
        }

        public async Task<int> deleteFile(int requestWiseFileId)
        {
            RequestWiseFile requestWiseFile = _requestWiseFileRepository.getFilesByrequestWiseFileId(requestWiseFileId);
            requestWiseFile.IsDeleted = new BitArray(1, true);
            return await _requestWiseFileRepository.updateRequestWiseFile(requestWiseFile) ? requestWiseFile.RequestId : 0;
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
