﻿using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementation
{
    public class PhysicianRepository : IPhysicianRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public PhysicianRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Physician> getAllPhysicians()
        {
            return _dbContext.Physicians.ToList();
        }
    }
}
