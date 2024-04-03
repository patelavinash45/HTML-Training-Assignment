using Services.ViewModels.Admin;

namespace Services.Interfaces.AdminServices
{
    public interface IProvidersService
    {
        Provider getProviders(int regionId);

        Task<bool> editProviderNotification(int providerId, bool isNotification);

        bool contactProvider(ContactProvider model);

        CreateProvider getCreateProvider();

        Task<bool> createProvider(CreateProvider model);

        ProviderScheduling getProviderSchedulingData();

        List<SchedulingTable> getSchedulingTableDate(int regionId, int type, string time);

        Task<bool> createShift(CreateShift model,int aspNetUserId);

        RequestedShift getRequestedShift();

        RequestShiftModel getRequestShiftTableDate(int regionId, bool isMonth, int pageNo);

        Task<bool> chnageShiftDetails(string dataList,bool isApprove);
    }
}
