using Microsoft.AspNetCore.Http;

namespace Services.Interfaces
{
    public interface IFileService
    {
        Task<int> addFile(IFormFile file, int requestId, string firstName, string lastName);
    }
}
