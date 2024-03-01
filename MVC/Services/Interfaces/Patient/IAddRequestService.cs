﻿using Repositories.ViewModels;

namespace Services.Interfaces.Patient
{
    public interface IAddRequestService
    {
        bool IsEmailExists(String email);

        AddRequestByPatient getModelForRequestByMe(int aspNetUserId);

        AddRequestByPatient getModelForRequestForSomeoneelse(int aspNetUserId);

        Task<bool> addPatientRequest(AddPatientRequest model);

        Task<bool> addRequestForMe(AddRequestByPatient model);

        Task<bool> addRequestForSomeOneelse(AddRequestByPatient model, int aspNetUserIdMe);

        Task<bool> addConciergeRequest(AddConciergeRequest model);

        Task<bool> addFamilyFriendRequest(AddFamilyRequest model);

        Task<bool> addBusinessRequest(AddBusinessRequest model);
    }
}
