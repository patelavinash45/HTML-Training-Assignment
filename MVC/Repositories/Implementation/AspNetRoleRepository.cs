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
    public class AspNetRoleRepository : IAspNetRoleRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public AspNetRoleRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool checkUserRole(string role)
        {
            AspNetRole aspNetRole = _dbContext.AspNetRoles.FirstOrDefault(a => a.Name.Trim() == "Patient");
            if(aspNetRole == null) {
                return false;
            }
            return true;
        }

        public async Task<bool> addUserRole(string role)
        {
            AspNetRole aspNetRole = new()
            {
                Name = "Patient",
            };
            _dbContext.Add(aspNetRole);
            int temp = await _dbContext.SaveChangesAsync();
            return temp > 0 ? true : false;
        }
    }
}
