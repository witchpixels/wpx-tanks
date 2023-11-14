using Godot;
using Witchpixels.Tanks.Entities.Generic.Components;
using Witchpixels.Tanks.Logging;

namespace Witchpixels.Tanks.Entities.Projectiles;

public partial class BulletController : CharacterBody3D
{
    private ILogger _logger;
    private VelocityComponent _velocityComponent;
    private int collisionCount = 0;
    
    public override async void _Ready()
    {
        base._Ready();
        _velocityComponent = GetNode<VelocityComponent>("components/VelocityComponent");
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        
        var direction = -GlobalTransform.Basis.Z;
        Velocity = _velocityComponent.GetVelocity();

        var collision = MoveAndCollide(_velocityComponent.GetVelocity());
        if (collision != null)
        {
            collisionCount++;
            
            direction = direction.Reflect(collision.GetNormal());
            LookAt(GlobalPosition + direction);

            var remainingDistance = collision.GetRemainder().Reflect(collision.GetNormal());
            MoveAndCollide(remainingDistance);
        }

        if (collisionCount > 2)
        {
            SetProcess(false);
        }
    }
}