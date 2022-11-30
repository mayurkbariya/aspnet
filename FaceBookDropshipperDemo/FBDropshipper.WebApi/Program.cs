using System;
using FBDropshipper.Application.Subscriptions.Commands.ImportSubscription;
using FBDropshipper.Application.UserSubscriptions.Commands.ProcessUserSubscription;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Initializer;
using FBDropshipper.WebApi;
using FBDropshipper.WebApi.Helper;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sentry;

try
{
    var builder = WebApplication.CreateBuilder(args);

    var startUp = new Startup(builder.Configuration);
// Add services to the container.

    #region Add To Service

    var services = builder.Services;
    startUp.ConfigureServices(services);

    #endregion

    #region Configure Service
    
    var urls = builder.Configuration.GetSection("Urls")?.Value;
    if (urls != null && urls.IsNotNullOrWhiteSpace())
    {
        //builder.WebHost.UseUrls(urls);
    }
    var app = builder.Build();

    startUp.Configure(app, app.Environment);
    using (IServiceScope scope = app.Services.CreateScope())
    {
        try
        {
            scope.ServiceProvider.GetService<ApplicationDbContext>()?.Database.Migrate();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            DatabaseInitializer.Initialize(context);
            RecurringJob.AddOrUpdate<HangfireMediatorHelper>("Subscription", (p) =>
                    p.Send(new ProcessUserSubscriptionRequestModel())
                , "0 * * * *");
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            var result = mediator?.Send(new ImportSubscriptionRequestModel()).Result;
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureMessage(ex.Message,SentryLevel.Error);
            //logger.Log(NLog.LogLevel.Error, "Error Occured When Initializing Database \n" + ex.Message);
        }
    }
    app.Run();

    #endregion
}
catch (Exception e)
{
    SentrySdk.CaptureMessage(e.Message, SentryLevel.Error);
}
finally
{
}