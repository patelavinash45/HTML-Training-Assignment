using Repositories.ViewModels;

namespace Services.Interfaces.Patient
{
    public interface IAddRequestService
    {
        Task<bool> addPatientRequest(AddPatientRequest model);
    }
}
