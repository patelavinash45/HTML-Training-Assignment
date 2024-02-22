using Repositories.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IAspNetRoleRepository
    {
        int checkUserRole(string role);

        Task<int> addUserRole(AspNetRole aspNetRole);
    }
}
