using System.Threading.Tasks;
using ticketing_api.Models;

namespace ticketing_api.Infrastructure
{
    public interface IEmailSender
    {
        Task SendForgotPasswordEmailAsync(AppUser user, string token);

        Task SendNewUserEmailAsync(AppUser admin, AppUser user, string token);

        Task SendAdminAlertAsync(AppUser admin, string message);

        Task SendAdminAlertNewUserAsync(AppUser admin, AppUser user);

        Task SendAdminAlertDeleteUserAsync(AppUser admin, AppUser user);

        Task SendAdminAlertRegisterSuccessfulAsync(AppUser admin, AppUser user);

        Task SendAdminAlertUpdateUserAsync(AppUser admin, AppUser user);

        Task SendNotificationAsync(string subject, string message);
    }
}
