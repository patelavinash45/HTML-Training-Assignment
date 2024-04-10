using Repositories.DataModels;

namespace Repositories.Interfaces
{
    public interface IRequestClientRepository
    {
        List<Region> getAllRegions();

        List<CaseTag> getAllReason();

        List<RequestClient> getAllRequestClients();

        List<RequestClient> getRequestClientsBasedOnFilter(Func<RequestClient,bool> predicate);

        List<RequestClient> getRequestClientByStatus(List<int> status, int skip, string patientName, int regionId, int requesterTypeId);

        int countRequestClientByStatus(List<int> status);

        int countRequestClientByStatusAndFilter(List<int> status, string patientName, int regionId, int requesterTypeId);

        List<RequestClient> getAllRequestClientForUser(int userId);

        Task<bool> addRequestClient(RequestClient requestClient);

        RequestClient getRequestClientByRequestId(int requestId);

        RequestClient getRequestClientAndRequestByRequestId(int requestId);

        Task<bool> updateRequestClient(RequestClient requestClient);
    }
}