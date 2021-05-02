public class UFO : Enemy
{
    override public void Despawn()
    {
        base.Despawn();
        Pool<UFO>.i.ReturnToPool(this);
    }
}
