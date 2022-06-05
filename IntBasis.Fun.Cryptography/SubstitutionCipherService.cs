namespace IntBasis.Fun.Cryptography;

/// <summary>
/// Substitutes one token for another to produce clear text or cypher text
/// </summary>
public class SubstitutionCipherService
{
    private readonly ICharacterEncoderService characterEncoderService;

    public SubstitutionCipherService(ICharacterEncoderService characterEncoderService)
    {
        this.characterEncoderService = characterEncoderService ?? throw new ArgumentNullException(nameof(characterEncoderService));
    }

    public string ApplyCharacterSubstitution(string inputText, IDictionary<char, char> substitutionMapping)
    {
        return characterEncoderService.Encode(inputText, c => substitutionMapping[c]);
    }
}
