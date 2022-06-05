using Microsoft.Extensions.DependencyInjection;

namespace IntBasis.Fun.Cryptography.Tests;

public class IntegrationAttribute : BaseServiceProviderDataAttribute
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddFunCryptography();
    }
}