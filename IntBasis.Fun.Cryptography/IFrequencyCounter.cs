namespace IntBasis.Fun.Cryptography
{
    public interface IFrequencyCounter
    {
        FrequencyAnalysis GetFrequencyAnalysis(string cipherText, FrequencyAnalysisOptions? options);
    }
}