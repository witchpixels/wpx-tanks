using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Witchpixels.Tanks.Initialization;

public interface IDependencyBuilder
{
    IDependencyBuilder DependsOn<TService>(Action<TService> resolvedFunc);
    void WhenReady(Action readyFunc);
}

public partial class ServiceManager : Node, IServiceRegistry, IDependencyGraph
{
    private readonly Dictionary<Type, Func<object>> _factoryFunctionsByType = new();
    private readonly List<DependencyBuilder> _unmetDependencies = new();

    public void RegisterService<TService>(Func<TService> serviceFactory)
    {
        _factoryFunctionsByType.Add(typeof(TService), () => serviceFactory()!);
    }

    public IDependencyBuilder Require(string? name = null)
    {
        return new DependencyBuilder(name ?? Guid.NewGuid().ToString(), this);
    }

    public override void _Process(double delta)
    {
        for (var index = 0; index < _unmetDependencies.Count;)
        {
            var dependencyBuilder = _unmetDependencies[index];
            if (!dependencyBuilder.TryResolveAll())
            {
                GD.Print($"Waiting on dependency graph for {dependencyBuilder.Name}");
                ++index;
            }
        }

        base._Process(delta);
    }

    private class DependencyBuilder : IDependencyBuilder
    {
        private readonly ServiceManager _serviceManager;
        private readonly Dictionary<Type, Action<object>> _unmetResolutionsByType = new();
        private Action? _readyFunc;

        public DependencyBuilder(string name, ServiceManager serviceManager)
        {
            Name = name;
            _serviceManager = serviceManager;
        }

        public string Name { get; }

        public IDependencyBuilder DependsOn<TService>(Action<TService> resolvedFunc)
        {
            _unmetResolutionsByType[typeof(TService)] = o => resolvedFunc((TService)o);
            return this;
        }

        public void WhenReady(Action readyFunc)
        {
            if (_readyFunc != null)
                throw new InvalidOperationException($"{Name}:Ready Func can only be specified once per builder");

            _readyFunc = readyFunc;
            if (!TryResolveAll()) _serviceManager._unmetDependencies.Add(this);
        }

        public bool TryResolveAll()
        {
            var resolved = new List<Type>();

            foreach (var typeToResolve in _unmetResolutionsByType)
                if (_serviceManager._factoryFunctionsByType.TryGetValue(typeToResolve.Key, out var factory))
                {
                    resolved.Add(typeToResolve.Key);
                    typeToResolve.Value(factory.Invoke());
                }

            foreach (var type in resolved) _unmetResolutionsByType.Remove(type);

            if (!_unmetResolutionsByType.Any())
            {
                _readyFunc?.Invoke();
                _serviceManager._unmetDependencies.Remove(this);
                return true;
            }

            return false;
        }
    }
}