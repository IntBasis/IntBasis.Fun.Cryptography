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
