using Services.ViewModels;

namespace Services.Interfaces.PatientServices
{
    public interface IViewProfileService
    {
        ViewProfile getProfileDetails(int aspNetUserId);

        Task<bool> updatePatientProfile(ViewProfile model, int aspNetUserId);
    }
}
