using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IPoolable
{
    // Enums
    public enum Type { AsteroidBig, AsteroidMedium, AsteroidSmall, UFO }

    // Events
    public static event System.Action<Enemy> OnEnemySpawned;
    public static event System.Action<Enemy> OnEnemyDespawned;
    public static event System.Action<Enemy> OnPlayerKilledEnemy;

    // Inspector
    [Header("References")]
    [SerializeField] EnemyMovement movement;
    [SerializeField] EnemyHitbox hitbox;
    [SerializeField] EnemyShoot shoot;

    [Header("Spawn On Kill")]
    [SerializeField] float spread;
    [SerializeField] bool spawnOnKill;
    [SerializeField] Type enemySpawnedOnKill;
    [SerializeField] int spawnCountOnKill;

    [Header("Settings")]
    [SerializeField] int scoreValue;

    // Privates
    Rigidbody2D rb;
    bool markedForDespawn;

    // Properties
    public int SpawnCountOnKill { get { return spawnCountOnKill; } }
    public int ScoreValue { get { return scoreValue; } }

    virtual protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (movement && rb) movement.Init(rb);
        else Debug.LogError("Required components are null.");

        if (hitbox) hitbox.Init(this);
    }

    void FixedUpdate()
    {
        if (movement) movement.HandleFixedUpdate();
    }

    void Update()
    {
        if (shoot) shoot.HandleUpdate();
        if (markedForDespawn)
        {
            markedForDespawn = false;
            Despawn();
        }
    }
    
    virtual public void Spawn()
    {
        markedForDespawn = false;
        OnEnemySpawned?.Invoke(this);
        if (movement) movement.Restart();
        if (shoot) shoot.Restart();
    }

    public void Kill()
    {
        if (spawnOnKill && spawnCountOnKill > 0)
        {
            Queue<Vector2> spawnPositions = new Queue<Vector2>();
            for (int i = 0; i < spawnCountOnKill; i++)
            {
                spawnPositions.Enqueue(new Vector2(transform.position.x + Random.Range(-spread, spread), transform.position.y + Random.Range(-spread, spread)));
            }
            Game.i.SpawnEnemiesAtPositions(enemySpawnedOnKill, spawnCountOnKill, spawnPositions);
        }
        OnPlayerKilledEnemy(this);
        Despawn();
    }

    virtual public void Despawn()           // Will be despawned this frame.
    {
        OnEnemyDespawned?.Invoke(this);
    }

    public void MarkForDespawn()            // Will be despawned the next frame.
    {
        markedForDespawn = true;
    }

}
