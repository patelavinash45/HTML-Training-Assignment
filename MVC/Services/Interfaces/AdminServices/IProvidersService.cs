using Services.ViewModels.Admin;

namespace Services.Interfaces.AdminServices
{
    public interface IProvidersService
    {
        List<ProviderLocation> getProviderLocation();

        Provider getProviders(int regionId);

        Task<bool> editProviderNotification(int providerId, bool isNotification);

        Task<bool> contactProvider(ContactProvider model);

        CreateProvider getCreateProvider();

        Task<bool> createProvider(CreateProvider model);

        ProviderScheduling getProviderSchedulingData();

        List<SchedulingTable> getSchedulingTableDate(int regionId, int type, string date);

        SchedulingTableMonthWise monthWiseScheduling(int regionId, String dateString);

        Task<bool> createShift(CreateShift model,int aspNetUserId);

        RequestedShift getRequestedShift();

        RequestShiftModel getRequestShiftTableDate(int regionId, bool isMonth, int pageNo);

        Task<bool> changeShiftDetails(string dataList,bool isApprove);

        ViewShift getShiftDetails(int shiftDetailsId);

        Task<bool> EditShiftDetails(string data);
    }
}
