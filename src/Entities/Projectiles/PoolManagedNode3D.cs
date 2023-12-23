using Godot;
using witchpixels.GodotObjectPool;

namespace Witchpixels.Tanks.Entities.Projectiles;

public partial class PoolManagedNode3D : Node3D, IPoolManaged
{
    public ulong Id { get; }
    public IPoolManaged.ReturnToPool OnReturnToPool { get; set; }
    public void Spawn()
    {
        throw new System.NotImplementedException();
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }
}