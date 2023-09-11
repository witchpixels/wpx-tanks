using Godot;
using Witchpixels.Tanks.Entities.Generic.Components;
using Witchpixels.Tanks.Entities.Tank.Visuals;
using Witchpixels.Tanks.Initialization;
using Witchpixels.Tanks.Logging;

namespace Witchpixels.Tanks.Entities.Tank.TankControllers;

public partial class PlayerTankController : CharacterBody3D
{
    private ILogger _logger;
    private TankVisualsComponent _tankVisualsComponent;
    private VelocityComponent _velocityComponent;
    private MouseWorldPositionComponent _mouseWorldPositionComponent;
    
    public override async void _Ready()
    {
        await IOC.DependencyGraph.Require(nameof(PlayerTankController))
            .DependsOn<ILoggerFactory>(
                l => _logger = l.CreateInstanceLogger<PlayerTankController>(
                    GetInstanceId().ToString()))
            .WaitOnReady();

        _tankVisualsComponent = GetNode<TankVisualsComponent>("components/TankVisualComponent");
        _velocityComponent = GetNode<VelocityComponent>("components/VelocityComponent");
        _mouseWorldPositionComponent = GetNode<MouseWorldPositionComponent>("components/MouseWorldPositionComponent");

        // wpx2023 TODO: This should be eventually changed to reflect the player# for couch/online co-op
        _tankVisualsComponent.TankColor = Colors.Blue;

        base._Ready();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        
        _tankVisualsComponent.SetTurretGlobalLook(_mouseWorldPositionComponent.MouseWorldPosition);
        
        if (_velocityComponent.Direction.LengthSquared() > float.Epsilon)
            _tankVisualsComponent.SetHullLocalLook(GlobalPosition + _velocityComponent.GetVelocity());
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        var movementVector = new Vector3(
            Input.GetAxis("p1_control_left", "p1_control_right"),
            0f,
            Input.GetAxis("p1_control_up", "p1_control_down"));

        _velocityComponent.Direction = movementVector;
        Velocity = _velocityComponent.GetVelocity();
        MoveAndSlide();

        _velocityComponent.Direction = Velocity.Normalized();
    }
}