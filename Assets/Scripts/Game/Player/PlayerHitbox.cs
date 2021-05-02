using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(PolygonCollider2D))]
public class PlayerHitbox : MonoBehaviour
{
    // Events
    public static event Action OnPlayerHit;
    void PlayerHit() => OnPlayerHit?.Invoke();

    // Inspector
    [Header("References")]
    [SerializeField] PlayerHitboxShield shield;

    //  Privates
    Collider2D hitboxCollider;
    HashSet<Collider2D> collisions = new HashSet<Collider2D>();


    #region Methods

    void Awake()
    {
        hitboxCollider = GetComponent<PolygonCollider2D>();
        if (!shield) Debug.LogError("HitboxShield isn't assigned.");
    }

    public void Activate()
    {
        hitboxCollider.enabled = true;
    }

    public void Deactivate()
    {
        hitboxCollider.enabled = false;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (shield.IsActive) return;
        bool wasHit = false;
        if (col.CompareTag("EnemyHitbox")) wasHit = true;
        if (col.CompareTag("Projectile"))
        {
            Projectile proj = col.GetComponent<Projectile>();
            if (proj.CanDamagePlayer)
            {
                wasHit = true;
                proj.Despawn();
            }
        }

        if (wasHit && !shield.IsActive) PlayerHit();
    }
    #endregion
}
