using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;

namespace Repositories.Implementation
{
    public class BusinessRepository : IBusinessRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public BusinessRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> addBusiness(Business business)
        {
            _dbContext.Add(business);
            await _dbContext.SaveChangesAsync();
            return business?.BusinessId ?? 0;
        }
    }
}
