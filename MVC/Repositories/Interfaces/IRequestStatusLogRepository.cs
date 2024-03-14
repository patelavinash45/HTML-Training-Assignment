﻿using Repositories.DataModels;

namespace Repositories.Interfaces
{
    public interface IRequestStatusLogRepository
    {
        List<RequestStatusLog> GetRequestStatusLogByRequestId(int requestId);

        Task<int> addRequestSatatusLog(RequestStatusLog requestStatusLog);

        Task<bool> updateRequestSatatusLog(RequestStatusLog requestStatusLog);

        Task<int> addBlockRequest(BlockRequest blockRequest);
    }
}
