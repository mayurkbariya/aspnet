using System.IO;
using System.Threading.Tasks;

namespace FBDropshipper.Application.Interfaces
{
    public interface IImageService
    {
        byte[] GetFile(string url);
        Task<string> DownloadAndSave(string url, string targetFolder = "img");
        Task<string> SaveImage(string base64, string targetFolder = "img", string extension = ".jpg");
        (string, string) SaveImageWithResize(string base64, string extension = ".jpg", string targetFolder = "img");
        Task DeleteImage(string url);
        string ResizeImage(string sourcePath, string pathToSave, int height);
        string ResizeImage(Stream stream, string pathToSave, int width, int height);
    }
}