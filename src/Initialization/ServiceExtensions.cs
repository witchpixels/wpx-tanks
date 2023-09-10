using System.Threading.Tasks;
using Witchpixels.Tanks.Logging;

namespace Witchpixels.Tanks.Initialization;

public static class ServiceExtensions
{
    public static async Task WaitOnReady(this IService service, ILogger logger)
    {
        while (!service.IsReady)
        {
            logger.Info("Waiting on {0}", service.ServiceName);
            await Task.Delay(1);
        }
    }
}