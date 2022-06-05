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
}
