using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interface;

namespace Repositories.Implementation
{
    public class AspNetUserRepository : IAspNetUserRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public AspNetUserRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int ValidateUser(String email, String password)
        {
            AspNetUser aspNetUser = _dbContext.AspNetUsers.FirstOrDefault(a => a.Email.Trim() == email.Trim() && a.PasswordHash==password);
            return aspNetUser?.Id ?? 0;
        }
    }
}

