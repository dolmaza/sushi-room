using System.Threading.Tasks;

namespace Sushi.Room.Application.Services
{
    public interface IUploadService
    {
        Task SaveImageAsync(string imageBase64, string newImageFileName, string oldImageFileName = default);

        void DeleteImage(string imageName);

        string GetImageUniqName(string imageName);
    }
}