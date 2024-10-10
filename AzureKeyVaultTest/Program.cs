using Azure.Identity;
using AzureKeyVaultTest;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

var hostBuilder = new HostBuilder();

hostBuilder.ConfigureLogging(x =>
{
    x.AddSimpleConsole(c =>
    {
        c.TimestampFormat = "dd/MM/yyyy HH:mm:ss";
        c.ColorBehavior = LoggerColorBehavior.Enabled;
    });
});

hostBuilder.ConfigureAppConfiguration(x =>
{
    x.AddJsonFile("appsettings.json", false, reloadOnChange: true);

    var config = x.Build();

    x.AddAzureAppConfiguration(options =>
    {
        options.Connect(config.GetConnectionString("AppConfig"));
        options.ConfigureKeyVault(kv =>
        {
            kv.SetCredential(new ClientSecretCredential(
                tenantId: config.GetValue<string>("SecretConfiguration:TenantId"),
                clientId: config.GetValue<string>("SecretConfiguration:ClientId"),
                clientSecret: config.GetValue<string>("SecretConfiguration:Secret")
            ));
        });
    });
});

hostBuilder.ConfigureServices(x =>
{
    x.AddHostedService<Service>();
});


var app = hostBuilder.Build();

await app.RunAsync();