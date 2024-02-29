using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;

namespace Repositories.Implementation
{
    public class AspNetUserRoleRepository : IAspNetUserRoleRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public AspNetUserRoleRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> addAspNetUserRole(AspNetUserRole aspNetUserRole)
        {
            _dbContext.Add(aspNetUserRole);
            int temp = await _dbContext.SaveChangesAsync();
            return temp>0? true: false;
        }

        public bool validateAspNetUserRole(int aspNetUserId, int userType)
        {
            AspNetUserRole aspNetUserRole = _dbContext.AspNetUserRoles.FirstOrDefault(a => a.UserId == aspNetUserId && a.RoleId == userType);
            return aspNetUserRole != null;
        }
    }
}
