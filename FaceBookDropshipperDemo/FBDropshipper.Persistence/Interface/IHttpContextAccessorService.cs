namespace FBDropshipper.Persistence.Interface
{
    public interface IHttpContextAccessorService
    {
        string GetUserId();
        object GetHttpContext();
    }
}