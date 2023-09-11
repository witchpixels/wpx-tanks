using System;

namespace Witchpixels.Tanks.Initialization;

public interface IDependencyBuilder
{
    IDependencyBuilder DependsOn<TService>(Action<TService> resolvedFunc);
    void WhenReady(Action readyFunc);
}