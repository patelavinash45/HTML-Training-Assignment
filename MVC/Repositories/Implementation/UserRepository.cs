using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;
using Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public UserRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int getUserID(int aspNetUserID)
        {
            User user = _dbContext.Users.FirstOrDefault(a => a.AspNetUserId == aspNetUserID);
            return user?.UserId ?? 0;
        }

        public async Task<int> addUser(AddPatientRequest model,int aspNetUserId)
        {
            User user = new()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Mobile = model.Mobile,
                Street = model.Street,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                AspNetUserId = aspNetUserId,
                CreatedBy = aspNetUserId,
                CreatedDate = DateTime.Now,
                House = model.House,
                IntYear = model.BirthDate.Value.Year,
                IntDate = model.BirthDate.Value.Day,
                StrMonth = model.BirthDate.Value.Month.ToString(),
            };
            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync();
            return user?. UserId ?? 0;
        }

        public User GetUser(int aspNetUserID)
        {
            return _dbContext.Users.FirstOrDefault(a => a.AspNetUserId == aspNetUserID);
        }

        public async Task<bool> updateProfile(ViewProfile model)
        {
            User user = GetUser((int)model.AspNetUserId);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Mobile = model.Mobile;
            user.Street = model.Street;
            user.City = model.City;
            user.State = model.State;
            user.ZipCode = model.ZipCode;
            user.IntDate = model.BirthDate.Value.Day;
            user.StrMonth = model.BirthDate.Value.Month.ToString();
            user.IntYear = model.BirthDate.Value.Year;
            _dbContext.Update(user);
            int temp=await _dbContext.SaveChangesAsync();
            return temp > 0;
        }
    }
}
