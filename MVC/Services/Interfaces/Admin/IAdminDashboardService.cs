using Repositories.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces.Admin
{
    public interface IAdminDashboardService
    {
        AdminDashboard getallRequests();
    }
}
