public class AsteroidMedium : Asteroid
{
    override public void Despawn()
    {
        base.Despawn();
        Pool_AsteroidMedium.i.ReturnToPool(this);
    }
}
