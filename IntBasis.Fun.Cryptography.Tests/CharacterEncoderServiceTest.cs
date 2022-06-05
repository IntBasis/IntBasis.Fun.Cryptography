using FluentAssertions;
using Xunit;

namespace IntBasis.Fun.Cryptography.Tests;

public class CharacterEncoderServiceTest
{
    [Theory(DisplayName = "Character Encoder: Empty"), AutoMoq]
    public void Empty(CharacterEncoderService subject)
    {
        var output = subject.Encode("", c => c);
        output.Should().BeEmpty();
    }

    [Theory(DisplayName = "Character Encoder: No Replacement"), AutoMoq]
    public void NoReplacement(CharacterEncoderService subject, string inputText)
    {
        var output = subject.Encode(inputText, c => c);
        output.Should().Be(inputText);
    }

    [Theory(DisplayName = "Character Encoder: To Uppercase"), AutoMoq]
    public void Replacement(CharacterEncoderService subject)
    {
        var inputText = "a B c D e";
        var output = subject.Encode(inputText, c => char.ToUpper(c));
        output.Should().Be("A B C D E");
    }
}
