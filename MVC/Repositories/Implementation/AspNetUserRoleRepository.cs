using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
