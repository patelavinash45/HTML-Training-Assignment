using Services.ViewModels.Admin;

namespace Services.Interfaces.AdminServices
{
    public interface IProvidersService
    {
        Provider getProviders(int regionId);

        Task<bool> editProviderNotification(int providerId, bool isNotification);

        bool contactProvider(ContactProvider model);

        CreateProvider GetCreateProvider();

        Task<bool> createProvider(CreateProvider model);

        ProviderScheduling GetProviderSchedulingData(int regionId);
    }
}
