using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IRegionRepository
    {
        int checkRegion(String regionName);

        Task<int> addRegion(String regionName);
    }
}
