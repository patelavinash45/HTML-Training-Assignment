using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;

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

        public async Task<int> addFile(RequestWiseFile requestWiseFile)
        {
            _dbContext.Add(requestWiseFile);
            await _dbContext.SaveChangesAsync();
            return requestWiseFile?.RequestWiseFileId ?? 0;
        }

        public List<RequestWiseFile> getFilesByrequestId(int requestId)
        {
            return _dbContext.RequestWiseFiles.Where(a => a.RequestId == requestId).ToList();
        }
    }
}
