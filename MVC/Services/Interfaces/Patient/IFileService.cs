using Microsoft.AspNetCore.Http;

namespace Services.Interfaces.Patient
{
    public interface IFileService
    {
        Task<int> addFile(IFormFile file, int requestId, String firstName, String lastName);
    }
}
