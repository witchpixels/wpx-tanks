using System;
using Godot;

namespace Witchpixels.Tanks.Entities.Tank.Visuals;

[Tool]
public partial class TankVisualsComponent : Node3D
{
    [Export]
    public Color TankColor
    {
        get => _color;
        set => OnColorChanged(value);
    }
    

    private Color _color = new (1f, 0f, 0f);
    
    private Node3D _cannon;
    private MeshInstance3D _turret;
    private MeshInstance3D _tankHull;
    private TankMeshShaderComponent? _tankShaderComponent;


    public override void _Ready()
    {
        _cannon = GetNode<Node3D>("cannon");
        _turret = GetNode<MeshInstance3D>("turret_conical");
        _tankHull = GetNode<MeshInstance3D>("tank_hull");
        _tankShaderComponent = GetNode<TankMeshShaderComponent>("components/mesh_shader");
        
        if (_tankHull == null) throw new Exception("Tank Hull is Missing!");
        if (_cannon == null) throw new Exception("Cannon is Missing!");
        if (_turret == null) throw new Exception("Turret is Missing!");
        if (_tankShaderComponent == null) throw new Exception("Tank Shader Component is missing!");
        
        OnColorChanged(_color);
        base._Ready();
    }

    public void SetTurretGlobalLook(Vector3 lookAt)
    {
        var lookVector = lookAt - GlobalPosition;
        lookVector.Y = 0;

        var lookDirection = lookVector.Normalized();

        _turret.LookAt(GlobalPosition + lookDirection);
        _cannon.LookAt(GlobalPosition + lookDirection);
    }

    public void SetHullLocalLook(Vector3 lookAt)
    {
        var lookVector = lookAt - GlobalPosition;
        lookVector.Y = 0;
        _tankHull.LookAt(GlobalPosition + lookVector.Normalized());
    }

    private void OnColorChanged(Color c)
    {
        if (_tankShaderComponent != null) _tankShaderComponent.Color = c;
        _color = c;
    }
}