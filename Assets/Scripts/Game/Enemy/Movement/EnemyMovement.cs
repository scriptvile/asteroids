using UnityEngine;

public abstract class EnemyMovement : WrapObject
{
    // Privates
    protected Rigidbody2D rb;
    virtual public void Init(Rigidbody2D rigidbody)
    {
        rb = rigidbody;
        InitBounds();
    }
    virtual public void Restart() { }

    virtual public void HandleFixedUpdate()     // Called by the Enemy.
    {
        WrapCheck();
    }
}
