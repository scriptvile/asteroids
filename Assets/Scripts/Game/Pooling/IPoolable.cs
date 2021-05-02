public interface IPoolable
{
    void Spawn();           // When the object is taken from the pool - it must be restarted in this method.
    void Despawn();         // Use this method to return the object to the pool.
}
