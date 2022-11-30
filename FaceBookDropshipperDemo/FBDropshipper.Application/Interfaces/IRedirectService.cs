using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBDropshipper.Application.Interfaces
{
    public interface IRedirectService
    {
        Task RedirectCall(Dictionary<string, string> parameters);
    }
}