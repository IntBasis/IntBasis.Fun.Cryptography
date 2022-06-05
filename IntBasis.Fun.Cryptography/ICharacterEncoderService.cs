namespace IntBasis.Fun.Cryptography;

public interface ICharacterEncoderService
{
    /// <summary>
    /// Returns the string yielded by applying the given <paramref name="encodeFunction"/> 
    /// to each character in the <paramref name="inputText"/>
    /// </summary>
    string Encode(string inputText, Func<char, char> encodeFunction);
}