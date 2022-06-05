using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace IntBasis.Fun.Cryptography.Tests;

public class SubstitutionCipherServiceTest
{
    [Fact(DisplayName = "Substitution: Empty")]
    public void Empty()
    {
        SubstitutionCipherService subject = new(new CharacterEncoderService());
        Dictionary<char, char> mapping = new();

        var output = subject.ApplyCharacterSubstitution("", mapping);

        output.Should().BeEmpty();
    }

    [Fact(DisplayName = "Substitution: One Matching Character")]
    public void OneMatchingChar()
    {
        SubstitutionCipherService subject = new(new CharacterEncoderService());
        Dictionary<char, char> mapping = new()
        {
            ['a'] = 'x'
        };

        var output = subject.ApplyCharacterSubstitution("aaa", mapping);

        output.Should().Be("xxx");
    }

    [Fact(DisplayName = "Substitution: Default Unmapped")]
    public void DefaultUnmapped()
    {
        SubstitutionCipherService subject = new(new CharacterEncoderService());
        Dictionary<char, char> mapping = new()
        {
            ['a'] = 'x'
        };

        var output = subject.ApplyCharacterSubstitution("abc", mapping);

        output.Should().Be("x--");
    }

    [Fact(DisplayName = "Substitution: Unmapped")]
    public void Unmapped()
    {
        SubstitutionCipherService subject = new(new CharacterEncoderService());
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
