using Repositories.DataModels;

namespace Repositories.Interfaces
{
    public interface IRequestClientRepository
    {
        List<Region> getAllRegions();

        List<CaseTag> getAllReason();

        List<RequestClient> getAllRequestClients();

        List<RequestClient> getRequestClientByStatus(int status, int skip, string patientName, int regionId, int requesterTypeId);

        int countRequestClientByStatus(int status);

        int countRequestClientByStatusAndFilter(int status, string patientName, int regionId, int requesterTypeId);

        List<RequestClient> getAllRequestClientForUser(int userId);

        Task<int> addRequestClient(RequestClient requestClient);

        RequestClient getRequestClientByRequestId(int requestId);

        RequestClient getRequestClientAndRequestByRequestId(int requestId);

        Task<bool> updateRequestClient(RequestClient requestClient);
    }
}