using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;

namespace Repositories.Implementation
{
    public class RequestRepository : IRequestRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public RequestRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Request> getAllRequest()
        {
            return _dbContext.Requests.ToList();
        }

        public async Task<int> addRequest(Request request)
        {
            _dbContext.Add(request);
            await _dbContext.SaveChangesAsync();
            return request?.RequestId ?? 0;
        }
    }
}
