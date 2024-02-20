using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.ViewModels;
using Repositories.ViewModels;

namespace Services.Interfaces
{
    public interface IDashboardService
    {
        List<Dashboard> GetUsersMedicalData(int aspNetUserId);
    }
}
