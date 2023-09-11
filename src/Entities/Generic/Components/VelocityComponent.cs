using Godot;
using Witchpixels.Tanks.Debug;

namespace Witchpixels.Tanks.Entities.Generic.Components;

[GlobalClass]
public partial class VelocityComponent : Node
{
    private Vector3 _direction = Vector3.Zero;

    [Export]
    public float Speed { get; set; }

    [Export]
    public Vector3 Direction
    {
        get => _direction;
        set => _direction = value.Normalized();
    }

    public Vector3 GetVelocity()
    {
        return Direction * Speed;
    }
    
#if DEBUG

    private DebugLine3D _debugLine;
    
    public override void _Ready()
    {
        _debugLine = new DebugLine3D();
        AddChild(_debugLine);
        base._Ready();
    }

    public override void _Process(double delta)
    {
        var parent = GetParent<Node3D>();
        _debugLine.StartPosition = parent.GlobalPosition;
        _debugLine.EndPosition = parent.GlobalPosition + GetVelocity();
        _debugLine.Color = Colors.Azure;
        base._Process(delta);
    }

#endif
}