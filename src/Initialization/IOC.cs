using System.Threading.Tasks;

namespace Witchpixels.Tanks.Initialization;

public static class IOC
{
#pragma warning disable CS8618
    public static IServiceRegistry Registry { get; set; }
    public static IDependencyGraph DependencyGraph { get; set; }

    public static async Task WaitOnReady()
    {
        while (Registry == null || DependencyGraph == null)
        {
            await Task.Delay(10);
        }
    }
#pragma warning restore CS8618
}