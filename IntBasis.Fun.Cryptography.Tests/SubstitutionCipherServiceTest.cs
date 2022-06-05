using FluentAssertions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace IntBasis.Fun.Cryptography.Tests;

public class SubstitutionCipherServiceTest
{
    [Theory(DisplayName = "Substitution: Empty"), Integration]
    public void Empty(SubstitutionCipherService subject)
    {
        Dictionary<char, char> mapping = new();

        var output = subject.ApplyCharacterSubstitution("", mapping);

        output.Should().BeEmpty();
    }

    [Theory(DisplayName = "Substitution: One Matching Character"), Integration]
    public void OneMatchingChar(SubstitutionCipherService subject)
    {
        Dictionary<char, char> mapping = new()
        {
            ['a'] = 'x'
        };

        var output = subject.ApplyCharacterSubstitution("aaa", mapping);

        output.Should().Be("xxx");
    }

    [Theory(DisplayName = "Substitution: Default Unmapped"), Integration]
    public void DefaultUnmapped(SubstitutionCipherService subject)
    {
        Dictionary<char, char> mapping = new()
        {
            ['a'] = 'x'
        };

        var output = subject.ApplyCharacterSubstitution("abc", mapping);

        output.Should().Be("x--");
    }

    [Theory(DisplayName = "Substitution: Unmapped"), Integration]
    public void Unmapped(SubstitutionCipherService subject)
    {
        Dictionary<char, char> mapping = new()
        {
            ['a'] = 'x',
            ['b'] = 'y',
            ['c'] = 'z',
        };
        var options = new SubstitutionOptions
        {
            DefaultForUnmapped = '?'
        };

        var output = subject.ApplyCharacterSubstitution("c.b.a!", mapping, options);

        output.Should().Be("z?y?x?");
    }

    [Theory(DisplayName = "Substitution: The Gold Bug"), Integration]
    public void TheGoldBug(SubstitutionCipherService substitutionCipherService, IFrequencyCounter frequencyCounter)
    {
        var path = Path.Combine("TestData", "GoldBug.txt");
        var cipherText = File.ReadAllText(path);
        // HACK: Remove new-lines
        cipherText = cipherText.ReplaceLineEndings("");
        var options = new FrequencyAnalysisOptions { IgnoreWhitespace = true };
        var frequencyAnalysis = frequencyCounter.GetFrequencyAnalysis(cipherText, options);
        // Now, in English, the letter which most frequently occurs is e.
        var e = frequencyAnalysis.TokensByFrequency.First();
        // Let us assume 8,then, as 'e'.
        e.Should().Be('8');
        // Now, of all the words in the language, 'the' is the most usual;
        // let us see, therefore, whether there are not repetitions of any three characters ...
        // the last of them being 8 ...
        // they will most probably represent the word 'the'
        var the = frequencyAnalysis.TrigramsByFrequency.First(trigram => trigram.EndsWith(e));
        Dictionary<char, char> mapping = new()
        {
            [the[0]] = 't',
            [the[1]] = 'h',
            [the[2]] = 'e'
        };

        var output = substitutionCipherService.ApplyCharacterSubstitution(cipherText, mapping, default);
        output.Should().Contain("t-eeth");
        // TODO: Going through the alphabet ... we arrive at the word 'tree' as the sole possible reading
        var r_i = output.IndexOf("t-eeth") + 1;
        var r = cipherText[r_i];
        mapping[r] = 'r';

        output = substitutionCipherService.ApplyCharacterSubstitution(cipherText, mapping, default);
        output.Should().Contain("thetreethr---h");
        // ...when the word 'through' makes itself evient at once.
        // But this discovery gives us three new letter 'o', 'u', and 'g'
        var o_i = output.IndexOf("thetreethr---h") + 10;
        var u_i = o_i + 1;
        var g_i = u_i + 1;
        var o = cipherText[o_i];
        var u = cipherText[u_i];
        var g = cipherText[g_i];
        mapping[o] = 'o';
        mapping[u] = 'u';
        mapping[g] = 'g';

        output = substitutionCipherService.ApplyCharacterSubstitution(cipherText, mapping, default);
        output.Should().Contain("thetreethrough");
    }
}
