using FluentAssertions;
using System.Collections.Generic;
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
}
