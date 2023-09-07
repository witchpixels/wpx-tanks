using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

// ReSharper disable once CheckNamespace
namespace witchpixels.GodotObjectPool;

public partial class ObjectPool<T> : Node, IObjectPool<T>
    where T : Node
{
    private readonly Dictionary<ulong, T> _alive = new();
    private readonly Stack<T> _dead = new();
    private readonly PackedScene _scene;


    public ObjectPool(int capacity, PackedScene scene)
    {
        _scene = scene;
        Capacity = capacity;
    }

    public int Capacity { get; }

    public int AliveCount => _alive.Count;
    public int DeadCount => _dead.Count;

    public T Retrieve()
    {
        T? next;

        if (_dead.Any())
        {
            next = _dead.Pop();
        }
        else
        {
            next = _scene.Instantiate() as T;
#if DEBUG
            if (next is null)
            {
                GD.PrintErr(
                    $"[wpx:pool#]: PackedScene {_scene.ResourcePath} did not resolve to type {typeof(T).FullName}");
                throw new Exception(
                    $"[wpx:pool#]: PackedScene {_scene.ResourcePath} did not resolve to type {typeof(T).FullName}");
            }
#endif
            
            next.Name = $"{typeof(T).Name} {_alive.Count}";

#if DEBUG
            if (_alive.Count >= Capacity)
                GD.PrintErr(
                    $"[wpx:pool#]: ObjectPool {_scene.ResourcePath} is over Capacity {_alive.Count - Capacity}");
#endif
        }

        _alive.Add(next.GetInstanceId(), next);

        return next;
    }

    public void Release(T node)
    {
        var instanceId = node.GetInstanceId();
        if (!_alive.ContainsKey(instanceId))
        {
            GD.PrintErr($"[wpx:pool#]: ObjectPool {_scene.ResourcePath} tried to free an object that it never owned");
            return;
        }

        _alive.Remove(instanceId);


        node.Reparent(this);
        node.SetProcess(false);

        _dead.Push(node);
    }

    public override void _Ready()
    {
        base._Ready();
        PreloadObjects();
    }

    private void PreloadObjects()
    {
        var name = typeof(T).Name;

        for (var i = 0; i < Capacity; ++i)
        {
            var node = _scene.Instantiate();

#if DEBUG
            if (node is not T)
            {
                GD.PrintErr(
                    $"[wpx:pool#]: PackedScene {_scene.ResourcePath} did not resolve to type {typeof(T).FullName}");
                throw new Exception(
                    $"[wpx:pool#]: PackedScene {_scene.ResourcePath} did not resolve to type {typeof(T).FullName}");
            }
#endif

            node.Name = $"{name} {i}";
            node.SetProcess(false);
            AddChild(node);
        }
    }
}