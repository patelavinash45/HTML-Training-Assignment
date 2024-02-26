using Repositories.ViewModels;

namespace Services.Interfaces.Patient
{
    public interface IViewDocumentsServices
    {
        ViewDocument getDocumentList(int requestId,int aspNetUserId);

        Task<int> uploadFile(ViewDocument model);

    }
}
