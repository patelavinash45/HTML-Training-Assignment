using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces.Patient
{
    public interface IFileService
    {
        Task<int> addFile(IFormFile file, int requestId, String firstName, String lastName);
    }
}
