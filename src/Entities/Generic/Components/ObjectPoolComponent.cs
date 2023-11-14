using System;
using Godot;
using witchpixels.GodotObjectPool;

namespace Witchpixels.Tanks.Entities.Generic.Components;

[GlobalClass]
public partial class ObjectPoolComponent : Node
{
    [Export] private int _maxObjectsToSpawn = 0;
    [Export] private PackedScene _spawnObjectScene = null;

    private ObjectPool<Node> _objectPool = null;
    private Node _objectSpawnContainer;
    private Action<Node> _setupFunc = _ => {};

    public void Setup<T>(Node objectSpawnContainer,
        Action<T> setupFunc)
        where T : Node
    {
        _objectSpawnContainer = objectSpawnContainer;
        _setupFunc = node => setupFunc(node as T);
        
        _objectPool = new ObjectPool<Node>(_maxObjectsToSpawn, _spawnObjectScene);
        AddChild(_objectPool);
    }

    public bool TrySpawnObject()
    {
        if (_objectPool.DeadCount <= 0) return false;
        var o = _objectPool.Retrieve();
        _setupFunc(o);
        o.Reparent(_objectSpawnContainer);
        return true;
    }
}