namespace IntBasis.Fun.Cryptography;

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
