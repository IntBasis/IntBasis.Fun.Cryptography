using System.Text;

namespace IntBasis.Fun.Cryptography;

/// <summary>
/// Runs mapping function over given input text to produce output text.
/// Could be used for encoding or decoding.
/// </summary>
public class CharacterEncoderService : ICharacterEncoderService
{
    /// <inheritdoc/>
    public string Encode(string inputText, Func<char, char> encodeFunction)
    {
        var sb = new StringBuilder(inputText.Length);
        foreach (var token in inputText)
        {
            var outputToken = encodeFunction(token);
            sb.Append(outputToken);
        }
        return sb.ToString();
    }
}
