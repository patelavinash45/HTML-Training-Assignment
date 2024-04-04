using Repositories.DataModels;
using Repositories.Interfaces;
using Services.Interfaces;
using Services.ViewModels;
using System.Collections;
using System.Net;
using System.Net.Mail;
using System.Text.Json;

namespace Services.Implementation
{
    public class ViewDocumentsServices : IViewDocumentsServices
    {
        private readonly IRequestWiseFileRepository _requestWiseFileRepository;
        private readonly IRequestClientRepository _requestClientRepository;
        private readonly IFileService _fileService;
        private readonly IUserRepository _userRepository;

        public ViewDocumentsServices(IRequestWiseFileRepository requestWiseFileRepository, IRequestClientRepository requestClientRepository,
                                      IFileService fileService, IUserRepository userRepository)
        {
            _requestWiseFileRepository = requestWiseFileRepository;
            _requestClientRepository = requestClientRepository;
            _fileService = fileService;
            _userRepository = userRepository;
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
            RequestClient requestClient = _requestClientRepository.getRequestClientByRequestId(requestId);
            Admin admin = _userRepository.getAdmionByAspNetUserId(aspNetUserId);
            ViewDocument viewDocument = new()
            {
                FirstName = requestClient.FirstName,
                LastName = requestClient.LastName,
                FileList = fileList,
            };
            return viewDocument;
        }

        public async Task<bool> uploadFile(ViewDocument model ,String firstName,String lastName,int requestId)
        {
            return await _fileService.addFile(requestId: requestId, firstName: firstName, lastName: lastName,file: model.File);
        }

        public async Task<bool> deleteFile(int requestWiseFileId)
        {
            RequestWiseFile requestWiseFile = _requestWiseFileRepository.getFilesByrequestWiseFileId(requestWiseFileId);
            requestWiseFile.IsDeleted = new BitArray(1, true);
            return await _requestWiseFileRepository.updateRequestWiseFile(requestWiseFile);
        }

        public async Task<bool> deleteAllFile(String requestWiseFileIds)
        {
            List<int> ids = JsonSerializer.Deserialize<List<String>>(requestWiseFileIds).Select(id => int.Parse(id)).ToList();
            foreach(var id in ids)
            {
                await deleteFile(id);
            }
            return true;
        }

        public bool sendFileMail(String requestWiseFileIds ,int requestId)
        {
            List<int> ids = JsonSerializer.Deserialize<List<String>>(requestWiseFileIds).Select(id => int.Parse(id)).ToList();
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("tatva.dotnet.avinashpatel@outlook.com"),
                Subject = "Document List",
                IsBodyHtml = true,
                Body = "All The Documents For RequestId : " + requestId.ToString(),
            };
            foreach (var id in ids)
            {
                RequestWiseFile requestWiseFile = _requestWiseFileRepository.getFilesByrequestWiseFileId(id);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files/"+requestWiseFile.RequestId.ToString());
                path = Path.Combine(path, requestWiseFile.FileName);
                mailMessage.Attachments.Add(new Attachment(path));
            }
            //RequestClient requestClient = _requestClientRepository.GetRequestClientByRequestId(requestId);
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
                smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
