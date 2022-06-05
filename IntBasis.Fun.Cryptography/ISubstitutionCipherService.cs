namespace IntBasis.Fun.Cryptography
{
    public interface ISubstitutionCipherService
    {
        string ApplyCharacterSubstitution(string inputText, IDictionary<char, char> substitutionMapping, SubstitutionOptions? options);
    }
}