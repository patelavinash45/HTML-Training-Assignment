using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;
using System.Collections;

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
            return _dbContext.RequestWiseFiles.Where(a => a.RequestId == requestId && a.IsDeleted 
                                                                                   == new BitArray(1, false)).ToList().Count;
        }

        public async Task<bool> addFile(RequestWiseFile requestWiseFile)
        {
            _dbContext.RequestWiseFiles.Add(requestWiseFile);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public List<RequestWiseFile> getFilesByrequestId(int requestId)
        {
            return _dbContext.RequestWiseFiles.Where(a => a.RequestId == requestId && a.IsDeleted == new BitArray(1, false)).ToList();
        }

        public RequestWiseFile getFilesByrequestWiseFileId(int requestWiseFileId)
        {
            return _dbContext.RequestWiseFiles.FirstOrDefault(a => a.RequestWiseFileId == requestWiseFileId);
        }

        public async Task<bool> updateRequestWiseFile(RequestWiseFile requestWiseFile)
        {
            _dbContext.RequestWiseFiles.Update(requestWiseFile);
            int temp = await _dbContext.SaveChangesAsync();
            return temp > 0;
        }
    }
}
