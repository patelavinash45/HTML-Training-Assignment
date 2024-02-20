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
    public class RequestWiseFileRepository : IRequestWiseFileRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public RequestWiseFileRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int countFile(int requestId)
        {
            List<RequestWiseFile> requestWiseFile = _dbContext.RequestWiseFiles.Where(a => a.RequestId == requestId).ToList();
            return requestWiseFile.Count;
        }
    }
}
