namespace FBDropshipper.Application.Interfaces
{
    public interface ISessionService
    {
        string GetTimeZone();
        string GetUserId();
        string GetRole();
        string GetTeamLeaderId();
        string GetTeamLeaderIdOrUserId();
        bool IsAdmin();
        string[] GetAllUserIds();
    }
}