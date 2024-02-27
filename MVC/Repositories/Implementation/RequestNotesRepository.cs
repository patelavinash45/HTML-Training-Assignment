using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;

namespace Repositories.Implementation
{
    public class RequestNotesRepository : IRequestNotesRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public RequestNotesRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public RequestNote GetRequestNoteByRequestId(int requestId)
        {
            return _dbContext.RequestNotes.FirstOrDefault(a => a.RequestId == requestId);
        }

        public async Task<int> addRequestNote(RequestNote requestNote)
        {
            _dbContext.Add(requestNote);
            await _dbContext.SaveChangesAsync();
            return requestNote?.RequestNotesId ?? 0;
        }

        public async Task<bool> updateRequestNote(RequestNote requestNote)
        {
            _dbContext.Update(requestNote);
            int temp= await _dbContext.SaveChangesAsync();
            return temp>0;
        }
    }
}
