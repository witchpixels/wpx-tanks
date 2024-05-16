using System.Diagnostics.CodeAnalysis;
using Godot;
using Witchpixels.Framework.Entities.Entity3D;
using Witchpixels.Framework.Injection;
using Witchpixels.Tanks.Entities.Generic.Components;
using Witchpixels.Tanks.Entities.Tank.Visuals;
using Witchpixels.Tanks.Initialization;
using Witchpixels.Tanks.Logging;

namespace Witchpixels.Tanks.Entities.Tank.TankControllers;

public partial class PlayerTankController : Character3DEntity
{
    private ILogger _logger;
    
    [RequireComponent] 
    private TankVisualsComponent _tankVisualsComponent = null!;
    
    [RequireComponent] 
    private VelocityComponent _velocityComponent = null!;
    
    [RequireComponent]
    private MouseWorldPositionComponent _mouseWorldPositionComponent = null!;
    
    public override async void _Ready()
    {
        base._Ready();
        await IOC.DependencyGraph.Require(nameof(PlayerTankController))
            .DependsOn<ILoggerFactory>(
                l => _logger = l.CreateInstanceLogger<PlayerTankController>(
                    GetInstanceId().ToString()))
            .WaitOnReady();
        
        // wpx2023 TODO: This should be eventually changed to reflect the player# for couch/online co-op
        _tankVisualsComponent.TankColor = Colors.Blue;
        _logger.Info("Player tank is ready");
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