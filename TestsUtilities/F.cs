using AutoFixture;
using AutoFixture.AutoMoq;

namespace TestsUtilities;

public static class F
{
    private static readonly IFixture Fixture = new Fixture().Customize(new AutoMoqCustomization());
        
    public static T Create<T>() => Fixture.Create<T>();

    public static TimeSpan CreateTimeSpanRoundedToSeconds() => TimeSpan.FromSeconds(F.Create<uint>());
    public static string FormatTimeSpanToHoconFormat(TimeSpan t) => $"{t.TotalSeconds}s";
}