using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

// ReSharper disable once CheckNamespace
namespace witchpixels.GodotObjectPool;

public class ObjectPool<T> : IObjectPool<T>
    where T : IPoolManaged
{
    public int Capacity { get; }
    
    private readonly Dictionary<ulong, T> _alive = new();
    private readonly Stack<T> _dead = new();
    private readonly Func<int, T> _objectFactoryFunc;

    public int AliveCount => _alive.Count;
    public int DeadCount => _dead.Count;

    public ObjectPool(int capacity, 
        Func<int, T> objectFactoryFunc)
    {
        Capacity = capacity;
        _objectFactoryFunc = objectFactoryFunc;
        PreloadObjects();
    }

    public T Retrieve()
    {
        T next;

        if (_dead.Any())
        {
            next = _dead.Pop();
        }
        else
        {
            next = Create(_alive.Count);
#if DEBUG
            if (_alive.Count >= Capacity)
                GD.PrintErr(
                    $"[wpx:pool#]: ObjectPool {typeof(T).FullName} is over Capacity {_alive.Count - Capacity}");
#endif
        }

        _alive.Add(next.Id, next);
        next.OnReturnToPool = () => Release(next);
        next.Spawn();

        return next;
    }

    public void Release(T node)
    {
        var instanceId = node.Id;
        if (!_alive.ContainsKey(instanceId))
        {
            GD.PrintErr($"[wpx:pool#]: ObjectPool {typeof(T).FullName} tried to free an object that it never owned");
            return;
        }

        _alive.Remove(instanceId);
        node.Reset();
        _dead.Push(node);
    }

    private void PreloadObjects()
    {
        for (var i = 0; i < Capacity; ++i)
        {
            Create(i);
        }
    }

    private T Create(int i)
    {
        var node = _objectFactoryFunc(i);
        node.Reset();
        _dead.Push(node);

        return node;
    }
}