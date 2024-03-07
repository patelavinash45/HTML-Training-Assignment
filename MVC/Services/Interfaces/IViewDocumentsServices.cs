using Services.ViewModels;

namespace Services.Interfaces
{
    public interface IViewDocumentsServices
    {
        ViewDocument getDocumentList(int requestId, int aspNetUserId);

        Task<int> uploadFile(ViewDocument model,String firstName,String lastName);

        Task<int> deleteFile(int requestWiseFileId);

        Task<int> deleteAllFile(List<int> requestWiseFileIds);

        Task<int> sendFileMail(List<int> requestWiseFileIds);

    }
}
