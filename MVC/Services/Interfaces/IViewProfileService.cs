﻿using Services.ViewModels;
using Services.ViewModels.Admin;

namespace Services.Interfaces
{
    public interface IViewProfileService
    {
        ViewProfile getProfileDetails(int aspNetUserId);

        Task<bool> updatePatientProfile(ViewProfile model, int aspNetUserId);

        AdminViewProfile GetAdminViewProfile(int aspNetUserId);

        Task<bool> editEditAdministratorInformastion(String data, int aspNetUserId);

        Task<bool> editMailingAndBillingInformation(String data, int aspNetUserId);
    }
}