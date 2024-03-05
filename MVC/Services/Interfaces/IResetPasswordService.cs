using Repositories.ViewModels;

namespace Services.Interfaces
{
    public interface IResetPasswordService
    {
        Task<bool> resetPasswordLinkSend(string email);

        SetNewPassword validatePasswordLink(string token, int aspNetUserId, string time);

        Task<bool> changePassword(SetNewPassword model);
    }
}
