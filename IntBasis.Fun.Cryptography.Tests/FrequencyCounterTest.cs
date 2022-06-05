using FluentAssertions;
using System.Collections.Generic;
using System.IO;
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
        frequencyAnalysis.TrigramsByFrequency.Should().BeEmpty();
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
        frequencyAnalysis.TrigramsByFrequency.Should().BeEmpty();
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
        frequencyAnalysis.TrigramsByFrequency.Should().BeEmpty();
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
        frequencyAnalysis.TrigramsByFrequency.Should().BeEmpty();
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
        frequencyAnalysis.TrigramsByFrequency.Should().Equal("abc");
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
        frequencyAnalysis.TrigramsByFrequency.Should().BeEmpty();
    }

    [Theory(DisplayName = "FrequencyCounter: Sort Tokens by Frequency (Ignore whitespace)"), AutoMoq]
    public void SortTokensByFrequencyNoWhitespace(FrequencyCounter frequencyCounter)
    {
        const string cipherText = "a b c c a c\n";
        var options = new FrequencyAnalysisOptions { IgnoreWhitespace = true };

        var frequencyAnalysis = frequencyCounter.GetFrequencyAnalysis(cipherText, options);

        frequencyAnalysis.TokenCount.Should().Equal(new Dictionary<char, int>
        {
            ['a'] = 2,
            ['b'] = 1,
            ['c'] = 3,
        });
        frequencyAnalysis.TokensByFrequency.Should().Equal('c', 'a', 'b');
        frequencyAnalysis.BigramsByFrequency.Should().BeEmpty();
        frequencyAnalysis.TrigramsByFrequency.Should().BeEmpty();
    }

    [Theory(DisplayName = "FrequencyCounter: Sort Bigrams by Frequency"), AutoMoq]
    public void SortBigramsByFrequency(FrequencyCounter frequencyCounter)
    {
        const string cipherText = "abc42abx42aby4242";

        var frequencyAnalysis = frequencyCounter.GetFrequencyAnalysis(cipherText);

        frequencyAnalysis.BigramsByFrequency.Should().Equal("42", "ab", "2a");
    }

    [Theory(DisplayName = "FrequencyCounter: Sort Bigrams by Frequency (Ignore whitespace)"), AutoMoq]
    public void SortBigramsByFrequencyNoWhitespace(FrequencyCounter frequencyCounter)
    {
        const string cipherText = " abc  42  a b x 4 \n\n\n 2  a b y 4 2 4 2  ";
        var options = new FrequencyAnalysisOptions { IgnoreWhitespace = true };

        var frequencyAnalysis = frequencyCounter.GetFrequencyAnalysis(cipherText, options);

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
        var options = new FrequencyAnalysisOptions { IgnoreWhitespace = true };

        var frequencyAnalysis = frequencyCounter.GetFrequencyAnalysis(cipherText, options);

        frequencyAnalysis.TokensByFrequency.Take(5)
                                            .Should()
                                            .Equal('e', 'n', 'a', 'o', 'r');
        frequencyAnalysis.BigramsByFrequency.Take(5)
                                            .Should()
                                            .Equal("in", "re", "er", "or", "ng");
        frequencyAnalysis.TrigramsByFrequency.Take(5)
                                             .Should()
                                             .Equal("ing", "app", "ppi", "pin", "ear");
    }

    [Theory(DisplayName = "FrequencyCounter: The Gold Bug"), AutoMoq]
    public void TheGoldBug(FrequencyCounter frequencyCounter)
    {
        var path = Path.Combine("TestData", "GoldBug.txt");
        var cipherText = File.ReadAllText(path);
        // "You observe there are no divisions between the words..." (though on 6 lines, so we should ignore newlines)
        var options = new FrequencyAnalysisOptions { IgnoreWhitespace = true };

        var frequencyAnalysis = frequencyCounter.GetFrequencyAnalysis(cipherText, options);

        // "My first step was to ascertain the perdominant letters,
        // as well as the least frequent. Count all, I constructed a table, thus..."
        var counts = frequencyAnalysis.TokenCount;
        counts['8'].Should().Be(33);
        counts[';'].Should().Be(26);
        counts['.'].Should().Be(1);
        // "Now, in English, the letter which most frequently occurs is e."
        frequencyAnalysis.TokensByFrequency.Take(5)
                                           .Should()
                                           .Equal('8', ';', '4', '‡', ')');
        // "Now, of all words in the language, 'the' is the most usual;
        frequencyAnalysis.BigramsByFrequency.Take(3)
                                            .Should()
                                            .Equal(";4", "48", "6*");
        // let us see, therefore, whether there are not repititions
        // of any three characters"
        frequencyAnalysis.TrigramsByFrequency.Should()
                                             .StartWith(";48") // the
                                             .And
                                             .Contain("5*†");  // and
    }
}
