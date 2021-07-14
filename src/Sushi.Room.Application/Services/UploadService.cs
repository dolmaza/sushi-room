using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Sushi.Room.Application.Options;

namespace Sushi.Room.Application.Services
{
    public class UploadService : IUploadService
    {
        private readonly AppSettings _appSettings;

        public UploadService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task SaveImageAsync(string imageBase64, string newImageFileName, string oldImageFileName = default)
        {
            DeleteImage(oldImageFileName);

            if (!string.IsNullOrEmpty(imageBase64))
            {
                await File.WriteAllBytesAsync(Path.Combine(_appSettings.UploadFolderPhysicalPath, newImageFileName), Convert.FromBase64String(imageBase64));
            }
        }

        public void DeleteImage(string imageName)
        {
            var oldImagePath = string.IsNullOrEmpty(imageName)
                ? default
                : Path.Combine(_appSettings.UploadFolderPhysicalPath, imageName);
            
            if (!string.IsNullOrEmpty(oldImagePath) && File.Exists(oldImagePath))
            {
                File.Delete(oldImagePath);
            }
        }

        public string GetImageUniqName(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
            {
                return default;
            }
            var extension = Path.GetExtension(imageName);
            var imageNameWithoutExtension = Path.GetFileNameWithoutExtension(imageName);

            return $"{imageNameWithoutExtension}-{Guid.NewGuid().ToString().Substring(0,8)}{extension}";
        }
    }
}