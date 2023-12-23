using System;
using System.Linq;
using Godot;
using witchpixels.GodotObjectPool;

namespace Witchpixels.Tanks.Entities.Generic.Components;

[Tool]
[GlobalClass]
public partial class ObjectPoolSpawnComponent : Node
{
    [Export] private int _maxObjectsToSpawn;
    [Export] private Node3D _objectToSpawn;

    private ObjectPool<IPoolManaged> _objectPool;
    private Node _objectSpawnContainer;
    private Action<Node3D> _spawnFunc;

    public void Setup(Node objectSpawnContainer, Action<Node3D> spawnFunc)
    {
        _objectToSpawn = GetChildren().First() as Node3D;
        _objectSpawnContainer = objectSpawnContainer;
        
        _objectToSpawn.SetProcess(false);
        _objectToSpawn.Hide();
        
        _objectPool = new ObjectPool<IPoolManaged>(
            _maxObjectsToSpawn,
            i =>
            {
                var n = _objectToSpawn.Duplicate();
                n.Name = _objectToSpawn.Name + i;
                
                var managedNode = (n as IPoolManaged);
                managedNode.Reset();

                AddChild(n);

                // ReSharper disable once SuspiciousTypeConversion.Global
                return managedNode;
            });
        _spawnFunc = n =>
        {
            n.Reparent(objectSpawnContainer);
            spawnFunc(n);
        };
    }

    public bool TrySpawnObject()
    {
        if (_objectPool.DeadCount <= 0) return false;
        var n = _objectPool.Retrieve();
        
        // ReSharper disable once SuspiciousTypeConversion.Global
        _spawnFunc(n as Node3D);
        return true;
    }

    public override string[] _GetConfigurationWarnings()
    {
        var nodes = GetChildren()
            .Where(x => x is Node3D)
            .Select(o => o.GetScript().Obj)
            .Where(x => x != null)
            .Any();
            //.Any(obj => obj.GetType().GetInterfaces().Contains(typeof(IPoolManaged)));
        
        if (!nodes)
        {
            return new[]
            {
                $"{nameof(ObjectPoolSpawnComponent)} must have a single child that is both a Node3D and implements IPoolManaged"
            };
        }
        return Array.Empty<string>();
    }
}