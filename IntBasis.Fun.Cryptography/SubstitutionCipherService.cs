namespace IntBasis.Fun.Cryptography;

public class SubstitutionOptions
{
    public char DefaultForUnmapped { get; set; }

    public static SubstitutionOptions Default => new()
    {
        DefaultForUnmapped = '-'
    };
}

/// <summary>
/// Substitutes one token for another to produce clear text or cypher text
/// </summary>
public class SubstitutionCipherService : ISubstitutionCipherService
{
    private readonly ICharacterEncoderService characterEncoderService;

    public SubstitutionCipherService(ICharacterEncoderService characterEncoderService)
    {
        this.characterEncoderService = characterEncoderService ?? throw new ArgumentNullException(nameof(characterEncoderService));
    }

    /// <inheritdoc/>
    public string ApplyCharacterSubstitution(string inputText, IDictionary<char, char> substitutionMapping, SubstitutionOptions? options = null)
    {
        options ??= SubstitutionOptions.Default;
        return characterEncoderService.Encode(inputText, token =>
        {
            if (substitutionMapping.ContainsKey(token))
                return substitutionMapping[token];
            return options.DefaultForUnmapped;
        });
    }
}
