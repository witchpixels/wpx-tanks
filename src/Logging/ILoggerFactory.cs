namespace Witchpixels.Tanks.Logging;

public interface ILoggerFactory
{
    public ILogger CreateServiceLogger<TService>();
    public ILogger CreateInstanceLogger<T>(string instanceId);
}