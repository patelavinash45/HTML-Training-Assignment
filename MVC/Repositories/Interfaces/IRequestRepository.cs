using Repositories.DataModels;

namespace Repositories.Interfaces
{
    public interface IRequestRepository
    {
        Task<int> addRequest(Request request);
    }
}
