using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace IntBasis.Fun.Cryptography.Tests;

public class FrequencyCounterTest
{
    [Theory(DisplayName = "FrequencyCounter: Construction"), AutoMoq]
    public void Construction(FrequencyCounter frequencyCounter)
    {
        frequencyCounter.Should().NotBeNull();
    }

    [Theory(DisplayName = "FrequencyCounter: Base Case"), AutoMoq]
    public void Base(FrequencyCounter frequencyCounter)
    {
        var frequencyAnalysis = frequencyCounter.GetFrequencyAnalysis("");
        frequencyAnalysis.TokenCount.Should().BeEmpty();
        frequencyAnalysis.TokensByFrequency.Should().BeEmpty();
        frequencyAnalysis.BigramsByFrequency.Should().BeEmpty();
    }

    [Theory(DisplayName = "FrequencyCounter: One Token"), AutoMoq]
    public void OneToken(FrequencyCounter frequencyCounter)
    {
        var frequencyAnalysis = frequencyCounter.GetFrequencyAnalysis("a");
        frequencyAnalysis.TokenCount.Should().Equal(new Dictionary<char, int>
        {
            ['a'] = 1
        });
        frequencyAnalysis.TokensByFrequency.Should().Equal('a');
        frequencyAnalysis.BigramsByFrequency.Should().BeEmpty();
    }

    [Theory(DisplayName = "FrequencyCounter: Three Tokens"), AutoMoq]
    public void ThreeTokens(FrequencyCounter frequencyCounter)
    {
        var frequencyAnalysis = frequencyCounter.GetFrequencyAnalysis("abc");
        frequencyAnalysis.TokenCount.Should().Equal(new Dictionary<char, int>
        {
            ['a'] = 1,
            ['b'] = 1,
            ['c'] = 1,
        });
        frequencyAnalysis.TokensByFrequency.Should().Equal('a', 'b', 'c');
        frequencyAnalysis.BigramsByFrequency.Should().BeEmpty();
    }

    [Theory(DisplayName = "FrequencyCounter: One Repeat Token"), AutoMoq]
    public void OneRepeatToken(FrequencyCounter frequencyCounter)
    {
        var frequencyAnalysis = frequencyCounter.GetFrequencyAnalysis("aa");
        frequencyAnalysis.TokenCount.Should().Equal(new Dictionary<char, int>
        {
            ['a'] = 2
        });
        frequencyAnalysis.TokensByFrequency.Should().Equal('a');
        frequencyAnalysis.BigramsByFrequency.Should().BeEmpty();
    }

    [Theory(DisplayName = "FrequencyCounter: Multiple Repeat Tokens"), AutoMoq]
    public void RepeatTokens(FrequencyCounter frequencyCounter)
    {
        var frequencyAnalysis = frequencyCounter.GetFrequencyAnalysis("abcabc");
        frequencyAnalysis.TokenCount.Should().Equal(new Dictionary<char, int>
        {
            ['a'] = 2,
            ['b'] = 2,
            ['c'] = 2,
        });
        frequencyAnalysis.TokensByFrequency.Should().Equal('a', 'b', 'c');
        frequencyAnalysis.BigramsByFrequency.Should().Equal("ab", "bc");
    }

    [Theory(DisplayName = "FrequencyCounter: Sort Tokens by Frequency"), AutoMoq]
    public void SortTokensByFrequency(FrequencyCounter frequencyCounter)
    {
        const string cipherText = "abccac";

        var frequencyAnalysis = frequencyCounter.GetFrequencyAnalysis(cipherText);

        frequencyAnalysis.TokenCount.Should().Equal(new Dictionary<char, int>
        {
            ['a'] = 2,
            ['b'] = 1,
            ['c'] = 3,
        });
        frequencyAnalysis.TokensByFrequency.Should().Equal('c', 'a', 'b');
        frequencyAnalysis.BigramsByFrequency.Should().BeEmpty();
    }

    [Theory(DisplayName = "FrequencyCounter: Sort Bigrams by Frequency"), AutoMoq]
    public void SortBigramsByFrequency(FrequencyCounter frequencyCounter)
    {
        const string cipherText = "abc42abx42aby4242";

        var frequencyAnalysis = frequencyCounter.GetFrequencyAnalysis(cipherText);

        frequencyAnalysis.BigramsByFrequency.Should().Equal("42", "ab", "2a");
    }

    [Theory(DisplayName = "FrequencyCounter: The Raven"), AutoMoq]
    public void TheRaven(FrequencyCounter frequencyCounter)
    {
        const string cipherText = @"
Once upon a midnight dreary, while I pondered, weak and weary,
Over many a quaint and curious volume of forgotten lore—
    While I nodded, nearly napping, suddenly there came a tapping,
As of some one gently rapping, rapping at my chamber door.
’Tis some visitor, I muttered, tapping at my chamber door—
    Only this and nothing more.";

        var frequencyAnalysis = frequencyCounter.GetFrequencyAnalysis(cipherText);

        frequencyAnalysis.TokensByFrequency.Take(6)
                                            .Should()
                                            .Equal(' ', 'e', 'n', 'a', 'o', 'r');
        // Excluding bigrams with white space
        frequencyAnalysis.BigramsByFrequency.Where(b => b.Trim().Length == 2)
                                            .Take(5)
                                            .Should()
                                            .Equal("in", "re", "er", "or", "ng");
    }
}
