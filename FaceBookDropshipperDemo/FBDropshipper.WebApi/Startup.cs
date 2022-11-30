using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using FBDropshipper.Application.Infrastructures;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.Users.Commands.RegisterUser;
using FBDropshipper.Common.Util;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Infrastructure.Option;
using FBDropshipper.Infrastructure.Service;
using FBDropshipper.Persistence.Context;
using FBDropshipper.WebApi.AuthRequirement;
using FBDropshipper.WebApi.Extension;
using FBDropshipper.WebApi.Filters;
using FBDropshipper.WebApi.Formatters;
using FBDropshipper.WebApi.Helper;
using FBDropshipper.WebApi.Hubs;
using FBDropshipper.WebApi.Services;
using FBDropshipper.WebApi.Services.Background;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.PostgreSql;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using Stripe;

namespace FBDropshipper.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                options.UseNpgsql(connectionString,
                        b => b.MigrationsAssembly("FBDropshipper.Persistence"))
                    .UseCamelCaseNamingConvention();
                options.UseOpenIddict();
            });
            services.AddIdentity<User, Role>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireDigit = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredLength = 4;
                    options.Lockout.MaxFailedAccessAttempts = 3;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromDays(2);
            });

            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore().UseDbContext<ApplicationDbContext>();
                })
                .AddServer(options =>
                {
                    options.UseAspNetCore().EnableTokenEndpointPassthrough()
                        .DisableTransportSecurityRequirement();
                    options.SetTokenEndpointUris("/connect/token");
                    options.AllowPasswordFlow();
                    options.AllowRefreshTokenFlow();
                    options.AcceptAnonymousClients();
                    // options.DisableHttpsRequirement(); // Note: Comment this out in production
                    options.RegisterScopes(
                        OpenIddictConstants.Scopes.OpenId,
                OpenIddictConstants.Scopes.Email,
                OpenIddictConstants.Scopes.Phone,
                OpenIddictConstants.Scopes.Profile,
                OpenIddictConstants.Scopes.OfflineAccess,
                        OpenIddictConstants.Scopes.Roles);
                    var basePath = PathUtil.AssemblyDirectory;// + "\\" + string.Format("TheRiceMill.Presentation");
                    string certificate = "Certificates/cert.pfx";
                    X509Certificate2 cert = new X509Certificate2(certificate, "qweasdzxc123",
                    X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.EphemeralKeySet);
                    options.AddSigningCertificate(cert);
                    options.AddEncryptionCertificate(cert);

                })
                .AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseAspNetCore();
                }); //Only compatible with the default token format. For JWT tokens, use the Microsoft JWT bearer handler.

            services.AddAuthentication(options =>
            {
                // options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                // options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                // options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                // options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("DevCorsPolicy", builder => builder
                    .WithOrigins("http://localhost:4200", "https://localhost:4200", "http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
                options.AddPolicy("ProdCorsPolicy", builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

            });

            // Add MediatR
            StripeConfiguration.ApiKey = (Configuration.GetSection("Stripe")["SecretKey"]);
            services.AddHttpContextAccessor();
            services.Configure<SendGridOption>(Configuration.GetSection("SendGrid"));
            services.Configure<S3Option>(Configuration.GetSection("S3"));
            services.Configure<StripeOption>(Configuration.GetSection("Stripe"));
            services.Configure<RainforestOption>(Configuration.GetSection("Rainforest"));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddMediatR(typeof(RegisterUserRequestHandler).GetTypeInfo().Assembly);
            services.AddTransient<IStripeService, StripeService>();
            services.AddTransient<ISessionService, SessionService>();
            services.AddSingleton<IS3AmazonService, S3AmazonService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IUrlService, UrlService>();
            services.AddHttpClient<IImageService, ImageService>();
            services.AddHttpClient<IRainforestApiService, RainforestApiService>();
            services.AddTransient<IAlertService, AlertService>();
            services
                .AddControllers(options =>
                {
                    options.Filters.Add(typeof(CustomAuthorizeFilter));
                    options.Filters.Add(typeof(CustomExceptionFilterAttribute));
                    options.Filters.Add(typeof(ValidationActionFilter));
                    options.Filters.Add(typeof(ObjectConversionFilter));
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                    options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
                })
                .AddFluentValidation(fv =>
                    fv.RegisterValidatorsFromAssemblyContaining<RegisterUserRequestModelValidator>());

            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo() { Title = "FB Dropshipper API", Description = Environment.GetEnvironmentVariable("BuildNumber"), Version = "v1" });
                var scheme = new OpenApiSecurityScheme()
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                };
                c.AddSecurityDefinition("Bearer", scheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {scheme, new string[] { }}
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(Configuration.GetConnectionString("HangfireConnection"), new PostgreSqlStorageOptions()
                {
                    QueuePollInterval = TimeSpan.FromMinutes(5),
                    JobExpirationCheckInterval = TimeSpan.FromMinutes(5),
                }).UseMediator());
            services.AddAuthorization(options =>
            {
                options.AddAllPolicies();
            });
            services.AddHangfireServer();
            services.AddSingleton<IBackgroundTaskQueueService, BackgroundTaskQueueService>();
            services.AddScoped<IAuthorizationHandler, RoleOrClaimRequirementHandler>();
            services.AddSignalR();
            services.AddLogging(conf =>
            {
                conf.AddConfiguration(Configuration);
                conf.AddSentry();
            });
            services.AddHostedService<QueuedHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // IdentityModelEventSource.ShowPII = true;
                // app.UseDeveloperExceptionPage();
                app.UseCors("DevCorsPolicy");
                // app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // app.UseHsts();

            }
            else
            {
                app.UseCors("ProdCorsPolicy");

            }
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            app.UseSwagger(x => x.SerializeAsV2 = true);

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FB Dropshipper API V1");
                c.RoutePrefix = "swagger";
            });
            // app.UseHttpsRedirection();



            app.UseStaticFiles();
            app.UseHangfireDashboard();
            if (!env.IsDevelopment())
            {
                // app.UseSpaStaticFiles();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("/notification");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapHangfireDashboard();

            });

            // app.UseSpa(spa =>
            // {
            //     // To learn more about options for serving an Angular SPA from ASP.NET Core,
            //     // see https://go.microsoft.com/fwlink/?linkid=864501
            //
            //     spa.Options.SourcePath = "ClientApp";
            //
            //     if (env.IsDevelopment())
            //     {
            //         // spa.UseAngularCliServer(npmScript: "start");
            //     }
            // });


        }

    }
}