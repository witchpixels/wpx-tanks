using System.Threading.Tasks;

namespace Witchpixels.Tanks.Initialization;

public static class DependencyBuilderExtensions
{
    public static async Task WaitOnReady(this IDependencyBuilder dependencyBuilder)
    {
        var done = false;
        dependencyBuilder.WhenReady(() => done = true);

        while (!done)
        {
            await Task.Delay(1);
        }
    } 
}