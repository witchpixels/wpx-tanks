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
        private set => OnColorChanged(value);
    }
    
    [Export]
    private TankMeshShaderComponent _tankShaderComponent;

    private Color _color = new (1f, 0f, 0f);
    
    private Node3D _cannon;
    private MeshInstance3D _turret;
    private MeshInstance3D _tankHull;


    public override void _Ready()
    {
        _cannon = GetNode<Node3D>("cannon");
        _turret = GetNode<MeshInstance3D>("turret_conical");
        _tankHull = GetNode<MeshInstance3D>("tank_hull");
        
        if (_tankHull == null) throw new Exception("Tank Hull is Missing!");
        if (_cannon == null) throw new Exception("Cannon is Missing!");
        if (_turret == null) throw new Exception("Turret is Missing!");
        if (_tankShaderComponent == null) throw new Exception("Tank Shader Component is missing!");
        
        base._Ready();
    }

    public void SetTurretGlobalLook(Vector3 lookAt)
    {
        _turret.LookAt(lookAt);
        _cannon.LookAt(lookAt);
    }

    public void SetHullGlobalLook(Vector3 lookAt)
    {
        _tankHull.LookAt(lookAt);
    }

    private void OnColorChanged(Color c)
    {
        _tankShaderComponent.Color = c;
        _color = c;
    }
}