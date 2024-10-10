using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using AzureKeyVaultTestCommon;

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

    //Adiciona o Azure KeyVault diretamente aos configuration providers
    //x.AddAzureKeyVaultDirectly(config);

    //Adiciona a App Configuration aos configuration providers junto ao resolver
    //para que seja possível ler os valores referenciados em um Azure KeyVault
    x.AddAzureAppConfigurationWithKeyVaultReference(config);
});

hostBuilder.ConfigureServices(x =>
{
    x.AddHostedService<Service>();
});

var app = hostBuilder.Build();

await app.RunAsync();