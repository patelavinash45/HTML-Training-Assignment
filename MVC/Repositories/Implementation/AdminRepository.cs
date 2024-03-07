﻿using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;

namespace Repositories.Implementation
{
    public class AdminRepository : IAdminRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public AdminRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Admin getAdmionByAspNetUserId(int aspNetUserId)
        {
            return _dbContext.Admins.FirstOrDefault(a => a.AspNetUserId == aspNetUserId);
        }
    }
}
