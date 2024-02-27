using Repositories.DataModels;

namespace Repositories.Interfaces
{
    public interface IRequestSatatusLogRepository
    {
        RequestStatusLog GetRequestStatusLogByRequestId(int requestId);
    }
}
