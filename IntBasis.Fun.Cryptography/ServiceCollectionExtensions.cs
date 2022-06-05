using IntBasis.Fun.Cryptography;

// The Microsoft convention is to put IServiceCollection extensions
// in the Microsoft.Extensions.DependencyInjection namespace
// to make consumption easy
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers services for IntBasis.Fun.Cryptography.
    /// <para/>
    /// 
    /// For external use:
    /// <list type="bullet">
    ///   <item><see cref="IFrequencyCounter"/></item>
    ///   <item><see cref="ISubstitutionCipherService"/></item>
    /// </list>
    /// <para/>
    /// 
    /// For internal use:
    /// <list type="bullet">
    ///   <item><see cref="ICharacterEncoderService"/></item>
    /// </list>
    /// 
    /// </summary>
    public static IServiceCollection AddFunCryptography(this IServiceCollection services)
    {
        return services.AddTransient<ICharacterEncoderService, CharacterEncoderService>()
                       .AddTransient<IFrequencyCounter, FrequencyCounter>()
                       .AddTransient<ISubstitutionCipherService, SubstitutionCipherService>();
    }
}
