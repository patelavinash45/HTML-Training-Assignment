﻿using Repositories.DataModels;

namespace Repositories.Interfaces
{
    public interface IRequestWiseFileRepository
    {
        int countFile(int requestId);

        Task<int> addFile(RequestWiseFile requestWiseFile);

        List<RequestWiseFile> getFilesByrequestId(int requestId);

        RequestWiseFile getFilesByrequestWiseFileId(int requestWiseFileId);

        Task<bool> updateRequestWiseFile(RequestWiseFile requestWiseFile);
    }
}
