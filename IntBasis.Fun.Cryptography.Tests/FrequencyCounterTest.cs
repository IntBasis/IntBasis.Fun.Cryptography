using FluentAssertions;
using Xunit;

namespace IntBasis.Fun.Cryptography.Tests;

public class FrequencyCounterTest
{
    [Theory(DisplayName = "FrequencyCounter: Construction"), AutoMoq]
    public void Construction(FrequencyCounter frequencyCounter)
    {
        frequencyCounter.Should().NotBeNull();
    }
}
