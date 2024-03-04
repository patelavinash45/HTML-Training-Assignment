using Repositories.DataModels;
using Repositories.ViewModels;

namespace Services.Interfaces
{
    public interface IViewDocumentsServices
    {
        ViewDocument getDocumentList(int requestId, int aspNetUserId);

        Task<int> uploadFile(ViewDocument model);

        Task<int> deleteFile(int requestWiseFileId);

        Task<int> deleteAllFile(List<RequestWiseFile> requestWiseFileIds);

    }
}
