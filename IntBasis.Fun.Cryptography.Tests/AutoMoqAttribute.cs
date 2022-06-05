using AutoFixture;
using AutoFixture.Xunit2;

namespace IntBasis.Fun.Cryptography.Tests;

public class AutoMoqAttribute : AutoDataAttribute
{
    public AutoMoqAttribute() : base(() => new Fixture())
    {
    }
}