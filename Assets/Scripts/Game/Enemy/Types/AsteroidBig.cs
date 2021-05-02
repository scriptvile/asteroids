using UnityEngine;

public class AsteroidBig : Asteroid
{
    // Privates
    Animator animator;


    override protected void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    override public void Spawn()
    {
        base.Spawn();
        animator.Play("spawn", 0, 0);
    }

    override public void Despawn()
    {
        base.Despawn();
        Pool_AsteroidBig.i.ReturnToPool(this);
    }
}