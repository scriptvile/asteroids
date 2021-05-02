using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    Enemy enemy;

    public void Init(Enemy enemy)
    {
        this.enemy = enemy;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (enemy == null)
        {
            Debug.LogWarning("Enemy Hitbox has not been initialized. Reference assigned?");
            return;
        }

        if (col.CompareTag("Projectile"))
        {
            Projectile projectile = col.GetComponent<Projectile>();
            if (projectile != null)
            {
                if (!projectile.BelongsToEnemy)
                {
                    if (!projectile.HasCollided)            // This is to prevent destroying two objects at once with a single projectile.
                    {
                        enemy.Kill();
                        projectile.MarkAsCollided();
                        projectile.Despawn();
                    }
                }
            }
        }
    }
}
