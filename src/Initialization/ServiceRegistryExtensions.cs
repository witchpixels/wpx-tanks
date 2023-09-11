namespace Witchpixels.Tanks.Initialization;

public static class ServiceRegistryExtensions
{
    public static void RegisterService<T>(this IServiceRegistry serviceRegistry, T service)
    {
        serviceRegistry.RegisterService(() => service);
    }
}