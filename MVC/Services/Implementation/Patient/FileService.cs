using Microsoft.AspNetCore.Http;
using Repositories.DataModels;
using Repositories.Interfaces;
using Services.Interfaces.Patient;

namespace Services.Implementation.Patient
{
    public class FileService : IFileService
    {
        private readonly IRequestWiseFileRepository _requestWiseFileRepository;

        public FileService( IRequestWiseFileRepository requestWiseFileRepository)
        {
            _requestWiseFileRepository = requestWiseFileRepository;
        }

        public async Task<int> addFile(IFormFile file, int requestId, String firstName, String lastName)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            };
            FileInfo fileInfo = new FileInfo(file.FileName);
            string fileName = requestId + "_" + fileInfo.Name;
            string fileNameWithPath = Path.Combine(path, fileName);
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            RequestWiseFile requestWiseFile = new()
            {
                RequestId = requestId,
                FileName = fileName,
                CreatedDate = DateTime.Now,
                Uploder = firstName + " " + lastName,
            };
            return await _requestWiseFileRepository.addFile(requestWiseFile);
        }
    }
}
