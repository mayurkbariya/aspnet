using System.Threading.Tasks;
using Hangfire;
using MediatR;
using Newtonsoft.Json;

namespace FBDropshipper.WebApi.Helper
{
    public static class HangfireConfigurationExtensions
    {

        public static IGlobalConfiguration UseMediator(this IGlobalConfiguration globalConfiguration)
        {
            var jsonSetting = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
            return globalConfiguration.UseSerializerSettings(jsonSetting);
        }
    }
    public static class MediatorExtension
    {
        public static void Enqueue<T>(this IMediator mediator, IRequest<T> request)
        {
            var backgroundJob = new BackgroundJobClient();
            backgroundJob.Enqueue<HangfireMediatorHelper>(p => p.Send(request));
        }
    }
    public class HangfireMediatorHelper
    {
        private readonly IMediator _mediator;
        public HangfireMediatorHelper(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Send<T>(IRequest<T> request)
        {
            await _mediator.Send(request);
        }
    }
}