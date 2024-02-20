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
    public class RequestClientRepository : IRequestClientRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public RequestClientRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<RequestClient> getAllRequestClient()
        {
            return _dbContext.RequestClients.ToList();
        }
    }
}
