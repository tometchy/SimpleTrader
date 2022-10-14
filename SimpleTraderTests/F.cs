using AutoFixture;
using AutoFixture.AutoMoq;

namespace SimpleTraderTests;

public static class F
{
    private static readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());
        
    public static T Create<T>() => _fixture.Create<T>();
}