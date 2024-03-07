using Repositories.DataModels;

namespace Repositories.Interfaces
{
    public interface IRequestClientRepository
    {
        List<RequestClient> getRequestClientByStatus(int status,int skip);

        int countRequestClientByStatus(int status);

        List<RequestClient> getAllRequestClientForUser(int userId);

        Task<int> addRequestClient(RequestClient requestClient);

        RequestClient GetRequestClientByRequestId(int requestId);

        Task<bool> updateRequestClient(RequestClient requestClient);

        List<RequestClient> getRequestClientByName(string firstName, string lastName);
    }
}
