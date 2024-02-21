using Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IViewProfileService
    {
        ViewProfile getProfileDetails(int aspNetUserId);

        Task<bool> updatePatientProfile(ViewProfile model);
    }
}
