using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YatirimKoc.Application.Interfaces;

namespace YatirimKoc.Infrastructure.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _env;

        public FileUploadService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<List<string>> UploadAsync(List<IFormFile> files, string folderName)
        {
            var uploadPath = Path.Combine(_env.WebRootPath, "uploads", folderName);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var uploadedPaths = new List<string>();

            foreach (var file in files)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(uploadPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                uploadedPaths.Add($"/uploads/{folderName}/{fileName}");
            }

            return uploadedPaths;
        }

        // YENİ EKLENEN SİLME METODU
        public Task<bool> DeleteAsync(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                return Task.FromResult(false);

            try
            {
                // URL'in başındaki '/' işaretini kaldırıp işletim sistemine uygun fiziksel yola çevirir.
                // Örn: /uploads/listings/resim.jpg -> C:\inetpub\wwwroot\uploads\listings\resim.jpg
                var relativePath = fileUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
                var physicalPath = Path.Combine(_env.WebRootPath, relativePath);

                // Dosya fiziksel olarak diskte varsa sil
                if (File.Exists(physicalPath))
                {
                    File.Delete(physicalPath);
                    return Task.FromResult(true);
                }

                return Task.FromResult(false);
            }
            catch
            {
                // Bir dosya başkası tarafından kullanılıyorsa vs. hata patlatmasın, silmeyi es geçsin
                return Task.FromResult(false);
            }
        }
    }
}
