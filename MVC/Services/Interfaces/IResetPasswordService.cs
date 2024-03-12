using Services.ViewModels;

namespace Services.Interfaces
{
    public interface IResetPasswordService
    {
        Task<bool> resetPasswordLinkSend(string email);

        SetNewPassword validatePasswordLink(string token);

        Task<bool> changePassword(SetNewPassword model);
    }
}
