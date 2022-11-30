using System.Threading.Tasks;

namespace FBDropshipper.Application.Interfaces
{
    public interface ISmsService
    {
        Task<bool> SendMessage(string phoneNumber, string message);
    }
}