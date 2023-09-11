using Godot;

namespace Witchpixels.Tanks.Entities.Generic.Components;

[GlobalClass]
public partial class MouseWorldPositionComponent : Node3D
{
    public Vector3 MouseWorldPosition { get; private set; }

    public override void _PhysicsProcess(double delta)
    {
        var mousePosition = GetViewport().GetMousePosition();
        var camera3d = GetViewport().GetCamera3D();


        var rayNormal = camera3d.ProjectRayNormal(mousePosition);
        var rayOrigin = camera3d.ProjectRayOrigin(mousePosition);

        var rayCast = PhysicsRayQueryParameters3D.Create(rayOrigin, rayNormal * 1000f);

        var rayCastResult = GetWorld3D().DirectSpaceState.IntersectRay(rayCast);

        if (rayCastResult.TryGetValue("position", out var pos))
        {
            MouseWorldPosition = pos.AsVector3();
        }

        base._PhysicsProcess(delta);
    }
}