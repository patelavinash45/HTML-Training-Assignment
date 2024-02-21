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

        public int validateUser(String email, String password)
        {
            AspNetUser aspNetUser = _dbContext.AspNetUsers.FirstOrDefault(a => a.Email.Trim() == email.Trim() && a.PasswordHash==password);
            return aspNetUser?.Id ?? 0;
        }

        public int checkUser(String email)
        {
            AspNetUser aspNetUser = _dbContext.AspNetUsers.FirstOrDefault(a => a.Email.Trim() == email.Trim());
            return aspNetUser?.Id ?? 0;
        }

        public async Task<int> addUser(String email, String password, String firstName, String mobile)
        {
            AspNetUser aspNetUser = new()
            {
                UserName = firstName,
                Email = email,
                PhoneNumber = mobile,
                PasswordHash = password,
                CreatedDate = DateTime.Now,
            };
            _dbContext.Add(aspNetUser);
            await _dbContext.SaveChangesAsync();
            return aspNetUser?.Id ?? 0;
        }
    }
}

