namespace Witchpixels.Tanks.Initialization;

public interface IDependencyGraph
{
    IDependencyBuilder Require(string? name = null);
}