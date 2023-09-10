using Godot;
using Godot.Collections;

namespace Witchpixels.Tanks.Entities.Tank.Visuals;

[Tool]
public partial class TankMeshShaderComponent : Node
{
    [Export]
    private ShaderMaterial _shaderMaterial;
    private Color _color = new(1f, 0f, 0f);
    
    [Export] public Color Color
    {
        get => _color;
        set => OnColorChanged(value);
    }

    [Export] private Array<MeshInstance3D> _shadableMeshes { get; set; } = new();

    private void OnColorChanged(Color c)
    {
        foreach (var shadableMesh in _shadableMeshes)
        {
            shadableMesh.MaterialOverlay = _shaderMaterial;
            shadableMesh.SetInstanceShaderParameter("tank_color", c);
            _color = c;
        }
    } 
}