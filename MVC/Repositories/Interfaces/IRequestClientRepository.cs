using Repositories.DataModels;

namespace Repositories.Interfaces
{
    public interface IRequestClientRepository
    {
        List<Region> getAllRegions();

        List<CaseTag> getAllReason();

        List<RequestClient> getAllRequestClients();

        List<RequestClient> getRequestClientByStatus(int status, int skip);

        int countRequestClientByStatus(int status);

        List<RequestClient> getRequestClientByName(string firstName, string lastName, int status, int skip);

        int countRequestClientByName(string firstName, string lastName, int status);

        List<RequestClient> getRequestClientByRegion(int regionId, int status, int skip);

        int countRequestClientByRegion(int regionId, int status);

        List<RequestClient> getRequestClientByRequesterType(int requestTypeId, int status, int skip);

        int countRequestClientByRequesterType(int requestTypeId, int status);

        List<RequestClient> getAllRequestClientForUser(int userId);

        Task<int> addRequestClient(RequestClient requestClient);

        RequestClient getRequestClientByRequestId(int requestId);

        RequestClient getRequestClientAndRequestByRequestId(int requestId);

        Task<bool> updateRequestClient(RequestClient requestClient);
    }
}