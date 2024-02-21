using Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAddRequestService
    {
        Task<bool> addPatientRequest(AddPatientRequest model);
    }
}
