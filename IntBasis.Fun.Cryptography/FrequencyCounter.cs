namespace IntBasis.Fun.Cryptography;

/// <summary>
/// Contains the Frequency Analysis result for a Cipher Text.
/// </summary>
public class FrequencyAnalysis
{
    /// <summary>
    /// Enables looking up the count of occurence of any token in the cipher text.
    /// The Key is the token and the Value is the number of occurrences.
    /// </summary>
    public IDictionary<char, int> TokenCount { get; }

    /// <summary>
    /// Ordered list of tokens from most frequent to least frequent.
    /// </summary>
    public IList<char> TokensByFrequency { get; }

    /// <summary>
    /// Ordered list of repeat bigrams from most frequent to least frequent.
    /// (Bigrams that occur only once are excluded.)
    /// See <see href="https://en.wikipedia.org/wiki/Bigram">Bigram - Wikipedia</see>.
    /// </summary>
    public IList<string> BigramsByFrequency { get; }

    /// <summary>
    /// Ordered list of repeat trigrams from most frequent to least frequent.
    /// (Trigrams that occur only once are excluded.)
    /// See <see href="https://en.wikipedia.org/wiki/Trigram">Trigram - Wikipedia</see>.
    /// </summary>
    public IList<string> TrigramsByFrequency { get; }

    /// <summary>
    /// Ordered list of doubled tokens (two of same token appearing successively)
    /// from most frequent to least frequent.
    /// </summary>
    public IList<string> Doubles { get; }

    public FrequencyAnalysis(IDictionary<char, int> tokenCount,
                             IList<char> tokensByFrequency,
                             IList<string> bigramsByFrequency,
                             IList<string> trigramsByFrequency,
                             IList<string> doubles)
    {
        TokenCount = tokenCount ?? throw new ArgumentNullException(nameof(tokenCount));
        TokensByFrequency = tokensByFrequency ?? throw new ArgumentNullException(nameof(tokensByFrequency));
        BigramsByFrequency = bigramsByFrequency ?? throw new ArgumentNullException(nameof(bigramsByFrequency));
        TrigramsByFrequency = trigramsByFrequency ?? throw new ArgumentNullException(nameof(trigramsByFrequency));
        Doubles = doubles ?? throw new ArgumentNullException(nameof(doubles));
    }
}

public class FrequencyAnalysisOptions
{
    /// <summary>
    /// If true, spaces, new-lines and other whitespace characters will not contribute to frequency analysis
    /// </summary>
    public bool IgnoreWhitespace { get; set; }

    public static FrequencyAnalysisOptions Default => new()
    {
        IgnoreWhitespace = false
        // TODO: IgnoreCase
    };
}

/// <summary>
/// Performs basic Frequency Analysis on ciphertext by counting tokens.
/// See <see href="https://en.wikipedia.org/wiki/Frequency_analysis">Frequency Analysis - Wikipedia</see>
/// </summary>
public class FrequencyCounter
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
