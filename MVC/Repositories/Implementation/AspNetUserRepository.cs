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

        public async Task<int> addUser(AspNetUser aspNetUser)
        {
            _dbContext.Add(aspNetUser);
            await _dbContext.SaveChangesAsync();
            return aspNetUser?.Id ?? 0;
        }

        public bool checkToken(String token, int aspNetUserId)
        {
            AspNetUser aspNetUser = _dbContext.AspNetUsers.FirstOrDefault(a => a.Id == aspNetUserId && a.ResetPasswordToken == token);
            return aspNetUser!=null?true:false;
        }

        public async Task<bool> setToken(String token, int aspNetUserId)
        {
            AspNetUser aspNetUser = _dbContext.AspNetUsers.FirstOrDefault(a => a.Id == aspNetUserId);
            if(aspNetUser!=null)
            {
                aspNetUser.ResetPasswordToken = token;
                _dbContext.Update(aspNetUser);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public AspNetUser getUser(int aspNetUserId)
        {
            return _dbContext.AspNetUsers.FirstOrDefault(a => a.Id == aspNetUserId);
        }

        public async Task<bool> changePassword(AspNetUser aspNetUser, String password)
        {
            aspNetUser.PasswordHash = password;
            _dbContext.Update(aspNetUser); 
            int temp = await _dbContext.SaveChangesAsync();
            return temp > 0;
        }
    }
}

