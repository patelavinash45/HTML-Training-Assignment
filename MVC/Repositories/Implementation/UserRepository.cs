﻿using Microsoft.EntityFrameworkCore;
using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;

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

        public async Task<int> addUser(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user?. UserId ?? 0;
        }

        public User getUser(int aspNetUserID)
        {
            return _dbContext.Users.FirstOrDefault(a => a.AspNetUserId == aspNetUserID);
        }

        public async Task<bool> updateProfile(User user)
        {
            _dbContext.Users.Update(user);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public Admin getAdmionByAspNetUserId(int aspNetUserId)
        {
            return _dbContext.Admins.FirstOrDefault(a => a.AspNetUserId == aspNetUserId);
        }

        public List<Physician> getAllPhysiciansByRegionId(int regionId)
        {
            return _dbContext.Physicians.Where(a => a.RegionId==regionId).ToList();
        }

        public Physician getPhysicianNameByPhysicianId(int physicianId)
        {
            return _dbContext.Physicians.FirstOrDefault(a => a.PhysicianId == physicianId);
        }

        public List<AdminRegion> getAdminRegionByAdminId(int adminId)
        {
            return _dbContext.AdminRegions.Where(a => a.AdminId == adminId).ToList();
        }

        public async Task<bool> updateAdmin(Admin admin)
        {
            _dbContext.Admins.Update(admin);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
