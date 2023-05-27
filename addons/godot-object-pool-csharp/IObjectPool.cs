using Godot;

namespace witchpixels.GodotObjectPool;

public interface IObjectPool<T> where T : Node
{
    public int Capacity { get; }
    public int AliveCount { get; }
    public int DeadCount { get; }

    public T Retrieve();
    public void Release(T node);

}