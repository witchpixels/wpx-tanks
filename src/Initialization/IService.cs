namespace Witchpixels.Tanks.Initialization;

public interface IService
{
    string ServiceName { get; }
    bool IsReady { get; }
}