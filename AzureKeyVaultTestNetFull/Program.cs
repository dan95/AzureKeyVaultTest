using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using AzureKeyVaultTestCommon;

namespace AzureKeyVaultTestNetFull
{
    public class Program
    {
        public static async Task Main()
        {
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
                x.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                x.AddJsonFile("appsettings.json");

                var config = x.Build();

                //Adiciona o Azure KeyVault diretamente aos configuration providers
                x.AddAzureKeyVaultDirectly(config);

                //Adiciona a App Configuration aos configuration providers junto ao resolver
                //para que seja possível ler os valores referenciados em um Azure KeyVault
                x.AddAzureAppConfigurationWithKeyVaultReference(config);
            });

            hostBuilder.ConfigureServices(x =>
            {
                x.AddHostedService<Service>();
            });

            await hostBuilder.RunConsoleAsync();
        }
    }
}
