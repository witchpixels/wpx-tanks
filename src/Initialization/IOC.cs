namespace Witchpixels.Tanks.Initialization;

public static class IOC
{
#pragma warning disable CS8618
    public static IServiceRegistry Registry { get; set; }
    public static IDependencyGraph DependencyGraph { get; set; }
#pragma warning restore CS8618
}