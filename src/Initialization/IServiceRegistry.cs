using System;

namespace Witchpixels.Tanks.Initialization;

public interface IServiceRegistry
{
    void RegisterService<TService>(Func<TService> serviceFactory);
}