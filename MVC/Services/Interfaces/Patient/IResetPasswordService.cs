using Repositories.ViewModels;

namespace Services.Interfaces.Patient
{
    public interface IResetPasswordService
    {
        Task<bool> resetPasswordLinkSend(string email);

        SetNewPassword validatePasswordLink(String token, int aspNetUserId, String time);

        Task<bool> changePassword(SetNewPassword model);
    }
}
