using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureKeyVaultRestApiTest;

public class AzureApiSettings
{
    [Required]
    public AzureApiKeyVaultSettings AzureApiKeyVaultSettings { get; set; } = default!;

    [Required]
    public AzureApiAuthorizationSettings AzureApiAuthorizationSettings { get; set; } = default!;
}

public class AzureApiAuthorizationSettings
{
    [Required]
    public string AuthorizationUri { get; set; } = default!;

    [Required]
    public string GrantType { get; set; } = default!;
}

public class AzureApiKeyVaultSettings
{
    [Required]
    public string TenantId { get; set; } = default!;

    [Required]
    public string ClientId { get; set; } = default!;

    [Required]

    public string AuthorizationScope { get; set; } = default!;

    [Required]
    public string ClientSecret { get; set; } = default!;

    [Required]
    public string KeyVaultUri { get; set; } = default!;
}