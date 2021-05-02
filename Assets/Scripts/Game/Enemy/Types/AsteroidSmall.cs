public class AsteroidSmall : Asteroid
{
    override public void Despawn()
    {
        base.Despawn();
        Pool_AsteroidSmall.i.ReturnToPool(this);
    }
}
