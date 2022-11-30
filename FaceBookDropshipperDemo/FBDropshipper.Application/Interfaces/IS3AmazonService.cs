using System.Threading.Tasks;

namespace FBDropshipper.Application.Interfaces
{
    public interface IS3AmazonService
    {
        Task<string> SaveImage(string base64);
        Task DeleteImage(string url);
    }
}