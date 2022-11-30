using System.Threading;
using System.Threading.Tasks;

namespace FBDropshipper.Application.Interfaces
{
    public interface IBackgroundTaskQueueService
    {
        void QueueBackgroundWorkItem(object workItem);
        void QueueBackgroundWorkItem(string key, object workItem);

        Task<object> DequeueAsync(
            CancellationToken cancellationToken);
    }
}