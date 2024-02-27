using Repositories.ViewModels;

namespace Services.Interfaces.Patient
{
    public interface IAddRequestService
    {
        bool IsEmailExists(String email);

        Task<bool> addPatientRequest(AddPatientRequest model);

        AddRequestByPatient getModelForRequestByMe(int aspNetUserId);

        Task<bool> addRequestForMe(AddRequestByPatient model);

        AddRequestByPatient getModelForRequestForSomeoneelse(int aspNetUserId);

        Task<bool> addRequestForSomeOneelse(AddRequestByPatient model, int aspNetUserIdMe);

        Task<bool> addConciergeRequest(AddConciergeRequest model);
    }
}
