using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;
using Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementation
{
    public class RequestWiseFileRepository : IRequestWiseFileRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public RequestWiseFileRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int countFile(int requestId)
        {
            List<RequestWiseFile> requestWiseFile = _dbContext.RequestWiseFiles.Where(a => a.RequestId == requestId).ToList();
            return requestWiseFile.Count;
        }

        public async Task<int> addFile(int requestId, AddPatientRequest model)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            };
            FileInfo fileInfo = new FileInfo(model.File.FileName);
            string fileName = requestId + "_" + fileInfo.Name;
            string fileNameWithPath = Path.Combine(path, fileName);
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                model.File.CopyTo(stream);
            }
            RequestWiseFile requestWiseFile = new()
            {
                RequestId = requestId,
                FileName = fileName,
                CreatedDate = DateTime.Now,
                Uploder = model.FirstName + " " + model.LastName,
            };
            _dbContext.Add(requestWiseFile);
            await _dbContext.SaveChangesAsync();
            return requestWiseFile?.RequestWiseFileId ?? 0;
        }
    }
}
