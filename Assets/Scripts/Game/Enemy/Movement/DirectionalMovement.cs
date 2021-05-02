using UnityEngine;

public class DirectionalMovement : EnemyMovement
{
    // Inspector
    [Header("Settings")]
    [SerializeField] float speed;

    override public void Init(Rigidbody2D rb)
    {
        base.Init(rb);
    }

    public override void Restart()
    {
        base.Restart();
        CreateForce();
    }

    override public void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    void CreateForce()
    {
        rb.velocity = Vector2.zero;
        rb.drag = 0;
        transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360f));
        rb.AddForce(transform.up * speed, ForceMode2D.Impulse);
    }
}
