using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;

namespace Repositories.Implementation
{
    public class AspNetRoleRepository : IAspNetRoleRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public AspNetRoleRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int checkUserRole(string role)
        {
            AspNetRole aspNetRole = _dbContext.AspNetRoles.FirstOrDefault(a => a.Name.Trim() == "Patient");
            return aspNetRole?.Id ?? 0;
        }

        public async Task<int> addUserRole(AspNetRole aspNetRole)
        {
            _dbContext.Add(aspNetRole);
            await _dbContext.SaveChangesAsync();
            return aspNetRole?.Id ?? 0;
        }
    }
}
