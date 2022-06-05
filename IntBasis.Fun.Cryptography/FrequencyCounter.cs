namespace IntBasis.Fun.Cryptography;

public class FrequencyAnalysis
{
    public IDictionary<char, int> TokenCount { get; }
    public IList<char> TokensByFrequency { get; }

    public FrequencyAnalysis(IDictionary<char, int> tokenCount, IList<char> tokensByFrequency)
    {
        TokenCount = tokenCount ?? throw new ArgumentNullException(nameof(tokenCount));
        TokensByFrequency = tokensByFrequency ?? throw new ArgumentNullException(nameof(tokensByFrequency));
    }
}

/// <summary>
/// Performs basic Frequency Analysis on ciphertext by counting tokens.
/// <see href="https://en.wikipedia.org/wiki/Frequency_analysis"/>
/// </summary>
public class FrequencyCounter
{
    /// <inheritdoc/>
    public FrequencyAnalysis GetFrequencyAnalysis(string cipherText)
    {
        var tokenCount = new Dictionary<char, int>();
        foreach (char token in cipherText)
        {
            if (!tokenCount.ContainsKey(token))
                tokenCount[token] = 1;
            else
                tokenCount[token]++;
        }
        var tokensByFrequency = tokenCount.OrderByDescending(kv => kv.Value)
                                            .Select(kv => kv.Key)
                                            .ToList();
        return new FrequencyAnalysis(tokenCount, tokensByFrequency);
    }
}
