using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Messaging.Data;
using Messaging.Hubs;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Messaging;

internal class Program
{
    private static async Task Main(string[] arguments)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args: arguments);
        _ = builder.Configuration.AddKeyPerFile(directoryPath: "/run/secrets");
        _ = builder.Services.AddControllersWithViews();
        _ = builder
            .Services.AddDataProtection()
            .PersistKeysToDbContext<ApplicationDatabaseContext>()
            .ProtectKeysWithCertificate(
                certificate: X509CertificateLoader.LoadPkcs12FromFile(
                    path: "/run/secrets/certificate-keys",
                    password: builder.Configuration["certificate-keys-password"]
                )
            );
        _ = builder
            .Services.AddDbContext<ApplicationDatabaseContext>(
                optionsAction: dbContextOptionsBuilder =>
                    dbContextOptionsBuilder.UseNpgsql(
                        connectionString: builder.Configuration.GetConnectionString(
                            name: "PostgresqlConnection"
                        )
                    )
            )
            .AddEndpointsApiExplorer()
            .AddLogging()
            .AddSwaggerGen(setupAction: swaggerGenOptions =>
            {
                swaggerGenOptions.IncludeXmlComments(
                    filePath: Path.Combine(
                        path1: AppContext.BaseDirectory,
                        path2: $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"
                    )
                );
                swaggerGenOptions.SwaggerDoc(
                    name: "messaging-api-v1",
                    info: new OpenApiInfo
                    {
                        Description = "An ASP.NET Core Web API for messaging",
                        Title = "Messaging API",
                        Version = "V1",
                    }
                );
            });
        _ = builder.Services.AddSignalR();
        _ = builder
            .WebHost.UseKestrel(options: kestrelServerOptions =>
            {
                kestrelServerOptions.ConfigureHttpsDefaults(
                    configureOptions: httpsConnectionAdapterOptions =>
                    {
                        httpsConnectionAdapterOptions.ServerCertificate =
                            X509CertificateLoader.LoadPkcs12FromFile(
                                path: "/run/secrets/certificate-messaging",
                                password: builder.Configuration["certificate-messaging-password"]
                            );
                        httpsConnectionAdapterOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls13;
                    }
                );
                kestrelServerOptions.ListenAnyIP(
                    port: 12800,
                    configure: listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
                        _ = listenOptions.UseHttps();
                    }
                );
            })
            .UseQuic();
        WebApplication app = builder.Build();
        _ = app.UseRouting()
            .UseStaticFiles()
            .UseSwagger()
            .UseSwaggerUI(setupAction: swaggerUIOptions =>
            {
                swaggerUIOptions.SwaggerEndpoint(
                    url: "messaging-api-v1/swagger.json",
                    name: "Messaging API V1"
                );
            })
            .UseWebSockets(
                options: new WebSocketOptions()
                {
                    KeepAliveInterval = TimeSpan.FromSeconds(seconds: 30),
                }
            );
        _ = app.UseAuthentication().UseAuthorization();
        _ = app.UseEndpoints(configure: endpointRouteBuilder =>
        {
            _ = endpointRouteBuilder.MapControllers();
            _ = endpointRouteBuilder.MapHub<ChatHub>(pattern: "/messaging/chat/websocket");
        });
        await app.RunAsync();
    }
}
