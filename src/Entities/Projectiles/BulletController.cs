using Godot;
using witchpixels.GodotObjectPool;
using Witchpixels.Tanks.Entities.Generic.Components;
using Witchpixels.Tanks.Logging;

namespace Witchpixels.Tanks.Entities.Projectiles;

public partial class BulletController : CharacterBody3D, IPoolManaged
{
    private VelocityComponent _velocityComponent;
    private int _collisionCount;
    
    public override async void _Ready()
    {
        base._Ready();
        Id = GetInstanceId();
        _velocityComponent = GetNode<VelocityComponent>("components/VelocityComponent");
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        
        var direction = -GlobalTransform.Basis.Z;
        Velocity = _velocityComponent.GetVelocity();

        var collision = MoveAndCollide(Velocity);
        if (collision != null)
        {
            _collisionCount++;
            
            direction = direction.Reflect(collision.GetNormal());
            LookAt(GlobalPosition + direction);

            var remainingDistance = collision.GetRemainder().Reflect(collision.GetNormal());
            MoveAndCollide(remainingDistance);
        }

        if (_collisionCount > 2)
        {
            OnReturnToPool?.Invoke();
        }
    }

    public ulong Id { get; private set; }
    public IPoolManaged.ReturnToPool OnReturnToPool { get; set; }
    public void Spawn()
    {
        SetProcess(true);
        Show();
    }

    public void Reset()
    {
        SetProcess(false);
        _collisionCount = 0;
        Hide();
    }
}