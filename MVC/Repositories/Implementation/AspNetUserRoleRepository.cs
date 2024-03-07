using Microsoft.EntityFrameworkCore;
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

        public AspNetUserRole validateAspNetUserRole(String email,String password, int userType)
        {
             return _dbContext.AspNetUserRoles.Include(a => a.User).Include(a => a.Role).
                        FirstOrDefault(a => a.RoleId == userType && a.User.Email==email && a.User.PasswordHash==password);
        }
    }
}
