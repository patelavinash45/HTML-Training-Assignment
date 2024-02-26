using Repositories.DataModels;

namespace Repositories.Interfaces
{
    public interface IRequestRepository
    {
        List<Request> getAllRequest();

        Task<int> addRequest(Request request);
    }
}
