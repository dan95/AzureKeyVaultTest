using Azure.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureKeyVaultTestCommon
{
    public static class AzureAppConfigurationExtensions
    {
        public static IConfigurationBuilder AddAzureKeyVaultDirectly(this IConfigurationBuilder builder, IConfiguration config)
        => builder.AddAzureKeyVault(new Uri(
        config.GetValue<string>("SecretConfiguration:KeyVaultUri")
        ), new ClientSecretCredential(
                tenantId: config.GetValue<string>("SecretConfiguration:TenantId"),
                clientId: config.GetValue<string>("SecretConfiguration:ClientId"),
                clientSecret: config.GetValue<string>("SecretConfiguration:Secret")
            ));

        public static IConfigurationBuilder AddAzureAppConfigurationWithKeyVaultReference(this IConfigurationBuilder builder, IConfiguration config)
            => builder.AddAzureAppConfiguration(options =>
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
    }
}
