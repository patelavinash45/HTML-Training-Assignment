using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;

namespace Repositories.Implementation
{
    public class BlockRequestsRepository : IBlockRequestsRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public BlockRequestsRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> addBlockRequest(BlockRequest blockRequest)
        {
            _dbContext.BlockRequests.Add(blockRequest);
            int temp = await _dbContext.SaveChangesAsync(); 
            return temp >0 ? blockRequest.BlockRequestId : 0;
        }
    }
}
