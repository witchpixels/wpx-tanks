using Godot;

namespace Witchpixels.Tanks.Debug;

[Tool]
[GlobalClass]
public partial class DebugLine3D : MeshInstance3D
{
    private Color _color = Colors.Red;
    private Vector3 _startPosition = Vector3.Zero;
    private Vector3 _endPosition = Vector3.Zero;
    private float _lineWidth = 1f;
    private bool _isDirty = true;

    private ImmediateMesh _mesh = new();

    [Export]
    public Color Color
    {
        get => _color;
        set
        {
            _color = value;
            _isDirty = true;
        }
    }

    [Export]
    public Vector3 StartPosition
    {
        get => _startPosition;
        set
        {
            _startPosition = value;
            _isDirty = true;
        }
    }

    [Export]
    public Vector3 EndPosition
    {
        get => _endPosition;
        set
        {
            _endPosition = value;
            _isDirty = true;
        }
    }

    public override void _Process(double delta)
    {
        if (_isDirty)
        {
            _mesh.ClearSurfaces();

            _mesh.SurfaceBegin(Mesh.PrimitiveType.Lines);

            _mesh.SurfaceSetColor(_color);
            _mesh.SurfaceAddVertex(_startPosition);
            _mesh.SurfaceAddVertex(_endPosition);

            _mesh.SurfaceEnd();

            Mesh = _mesh;

            _isDirty = false;
        }
    }
}