using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;

namespace Repositories.Implementation
{
    public class RequestBusinessRepository : IRequestBusinessRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public RequestBusinessRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> addRequestBusiness(RequestBusiness requestBusiness)
        {
            _dbContext.Add(requestBusiness);
            await _dbContext.SaveChangesAsync();
            return requestBusiness?.RequestBusinessId ?? 0;
        }
    }
}
