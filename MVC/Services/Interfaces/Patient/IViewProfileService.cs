using Repositories.ViewModels;

namespace Services.Interfaces.Patient
{
    public interface IViewProfileService
    {
        ViewProfile getProfileDetails(int aspNetUserId);

        Task<bool> updatePatientProfile(ViewProfile model);
    }
}
