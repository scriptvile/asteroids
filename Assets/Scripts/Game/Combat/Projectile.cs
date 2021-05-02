using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : WrapObject, IPoolable
{
    // Inspector
    [Header("Settings")]
    [SerializeField] float travelSpeed;
    [SerializeField] float lifetime;
    [Range(0f, 1f)] [SerializeField] float enemySlowFactor;                 // If this projectile belongs to the enemy - let it travel slower (%).
    [Range(0f, 2f)] [SerializeField] float enemyLifetimeIncreaseFactor;     // Slower projectiles die too quickly.

    // Privates
    float lifetimeLeft;
    Rigidbody2D rb;
    bool hasCollided;
    bool canDamagePlayer;
    bool belongsToEnemy;

    // Properties
    public bool HasCollided { get { return hasCollided; } }
    public bool CanDamagePlayer { get { return canDamagePlayer; } }
    public bool BelongsToEnemy { get { return belongsToEnemy; } }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        InitBounds();
    }

    public void Spawn()
    {
        hasCollided = false;
        lifetimeLeft = lifetime;
        rb.velocity = new Vector2(0, 0);
        canDamagePlayer = false;
    }

    void Update()
    {
        lifetimeLeft -= Time.deltaTime;
        if (lifetimeLeft <= 0) Despawn();
    }

    void FixedUpdate()
    {
        WrapCheck();
    }
    public void CreateForce(Quaternion rotation)
    {
        transform.rotation = rotation;
        rb.drag = 0;
        rb.velocity = Vector2.zero;
        float speed = travelSpeed;
        if (belongsToEnemy) speed = travelSpeed * (1 - enemySlowFactor);
        rb.AddForce(transform.up * speed, ForceMode2D.Impulse);
    }

    public void Despawn()
    {
        Pool_Projectile.i.ReturnToPool(this);
    }

    public void MarkAsCollided()
    {
        hasCollided = true;
    }

    public void SetOwner(bool isOwnerEnemy)
    {
        belongsToEnemy = isOwnerEnemy;
        if (belongsToEnemy)
        {
            canDamagePlayer = true;
            lifetimeLeft = lifetime + (lifetime * enemyLifetimeIncreaseFactor);
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (belongsToEnemy) return;
        if (col.CompareTag("Player")) canDamagePlayer = true;
        
    }
}
