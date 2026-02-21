using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YatirimKoc.Application.Interfaces
{
    public interface IFileUploadService
    {
        Task<List<string>> UploadAsync(List<IFormFile> files, string folderName);
    }

}
