using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Application.Common
{
    public class StorageServieces : IStorageServices
    {
        private readonly string _userContentFolder;
        private readonly string _USER_CONTENT_FOLDER = "user-content";

        public StorageServieces(IWebHostEnvironment webHostEnvironment)
        {
            _userContentFolder = Path.Combine(webHostEnvironment.WebRootPath, _USER_CONTENT_FOLDER);
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }

        public string GetFileUrl(string fileName)
        {
            return $"/{_USER_CONTENT_FOLDER}/{fileName}";
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }
    }
}