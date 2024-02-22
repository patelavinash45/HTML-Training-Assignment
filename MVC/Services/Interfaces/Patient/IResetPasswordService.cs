using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces.Patient
{
    public interface IResetPasswordService
    {
        bool resetPasswordLinkSend(string email);
    }
}
