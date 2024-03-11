﻿using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;

namespace Repositories.Implementation
{
    public class HealthProfessionalRepository : IHealthProfessionalRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public HealthProfessionalRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<HealthProfessionalType> getHealthProfessionalTypes()
        {
            return _dbContext.HealthProfessionalTypes.ToList();
        }

        public List<HealthProfessional> getHealthProfessionalByProfession(int professionId)
        {
            return _dbContext.HealthProfessionals.Where(a => a.Profession ==  professionId).ToList();
        }

        public HealthProfessional getHealthProfessional(int VenderId)
        {
            return _dbContext.HealthProfessionals.FirstOrDefault(a => a.VendorId == VenderId);
        }

        public async Task<int> addOrderDetails(OrderDetail orderDetail)
        {
            _dbContext.Add(orderDetail);
            await _dbContext.SaveChangesAsync();
            return orderDetail?.Id ?? 0;
        }
    }
}
