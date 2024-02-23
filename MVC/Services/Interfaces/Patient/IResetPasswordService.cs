using Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces.Patient
{
    public interface IResetPasswordService
    {
        Task<bool> resetPasswordLinkSend(string email);

        SetNewPassword validatePasswordLink(String token, int aspNetUserId, String time);

        Task<bool> changePassword(SetNewPassword model);
    }
}
