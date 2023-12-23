namespace witchpixels.GodotObjectPool;

public interface IPoolManaged
{
    ulong Id { get; }
    
    delegate void ReturnToPool();
    ReturnToPool OnReturnToPool { get; set; }

    void Spawn();
    void Reset();
}