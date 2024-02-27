using Repositories.DataModels;

namespace Repositories.Interfaces
{
    public interface IRequestNotesRepository
    {
        RequestNote GetRequestNoteByRequestId(int requestId);

        Task<int> addRequestNote(RequestNote requestNote);

        Task<bool> updateRequestNote(RequestNote requestNote);
    }
}
