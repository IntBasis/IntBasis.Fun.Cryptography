namespace IntBasis.Fun.Cryptography;

/// <summary>
/// Performs basic Frequency Analysis on ciphertext by counting tokens.
/// See <see href="https://en.wikipedia.org/wiki/Frequency_analysis">Frequency Analysis - Wikipedia</see>
/// </summary>
public class FrequencyCounter : IFrequencyCounter
{
    /// <inheritdoc/>
    public FrequencyAnalysis GetFrequencyAnalysis(string cipherText, FrequencyAnalysisOptions? options = null)
    {
        if (cipherText is null)
            throw new ArgumentNullException(nameof(cipherText));
        options ??= FrequencyAnalysisOptions.Default;
        var tokenCount = new Dictionary<char, int>();
        var bigramCount = new Dictionary<string, int>();
        var trigramCount = new Dictionary<string, int>();
        var doubleCount = new Dictionary<string, int>();
        char previousToken = default;
        char previousPreviousToken = default;
        foreach (char token in cipherText)
        {
            if (options.IgnoreWhitespace && char.IsWhiteSpace(token))
                continue;
            IncrementCounter(tokenCount, token);
            var bigram = $"{previousToken}{token}";
            IncrementCounter(bigramCount, bigram);
            var trigram = $"{previousPreviousToken}{previousToken}{token}";
            IncrementCounter(trigramCount, trigram);
            if (token == previousToken)
                IncrementCounter(doubleCount, bigram);
            previousPreviousToken = previousToken;
            previousToken = token;
        }
        var tokensByFrequency = OrderByCount(tokenCount);
        // Exclude n-grams that occur only once because they have no value
        var bigramsByFrequency = OrderByCount(bigramCount, greaterThanOne: true);
        var trigramsByFrequency = OrderByCount(trigramCount, greaterThanOne: true);
        var doubles = OrderByCount(doubleCount);
        return new FrequencyAnalysis(tokenCount,
                                     tokensByFrequency,
                                     bigramsByFrequency,
                                     trigramsByFrequency,
                                     doubles);
    }

    private static List<T> OrderByCount<T>(IEnumerable<KeyValuePair<T, int>> counts, bool greaterThanOne = false) where T : notnull
    {
        if (greaterThanOne)
            counts = counts.Where(kv => kv.Value > 1);
        return counts.OrderByDescending(kv => kv.Value)
                     .Select(kv => kv.Key)
                     .ToList();
    }

    private static void IncrementCounter<T>(Dictionary<T, int> tokenCount, T token) where T : notnull
    {
        if (!tokenCount.ContainsKey(token))
            tokenCount[token] = 1;
        else
            tokenCount[token]++;
    }
}
