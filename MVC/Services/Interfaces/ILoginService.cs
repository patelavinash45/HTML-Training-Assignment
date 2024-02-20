using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.ViewModels;

namespace Services.Interface
{
    public interface ILoginService
    {
        int auth(PatientLogin model);
    }
}
