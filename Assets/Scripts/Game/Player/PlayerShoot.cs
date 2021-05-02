using UnityEngine;
using System;

public class PlayerShoot : Shoot
{
    // Events
    public static event Action OnShoot;

    // Inspector
    [Header("Settings")]
    [SerializeField] KeyCode shootKey;

    // Privates
    float baseShootCooldown;
    bool isBoostActive;

    void Awake()
    {
        baseShootCooldown = shootCooldown;
    }

    void OnEnable()
    {
        Pool_Booster.OnFirerateBoostActivated += FirerateBoosterActivated;
        Pool_Booster.OnFirerateBoostDeactivated += FirerateBoosterDeactivated;
    }

    void OnDisable()
    {
        Pool_Booster.OnFirerateBoostActivated -= FirerateBoosterActivated;
        Pool_Booster.OnFirerateBoostDeactivated -= FirerateBoosterDeactivated;
    }

    override public void HandleUpdate()
    {
        base.HandleUpdate();

        if (isBoostActive)
        {
            if (Input.GetKey(shootKey)) ShootProjectile();
        }
        else
        {
            if (Input.GetKeyDown(shootKey))
                if (timeToNextShoot <= 0) ShootProjectile();
        }
    }

    override protected void ShootProjectile()
    {
        if (timeToNextShoot > 0) return;

        base.ShootProjectile();
        Projectile proj = Pool_Projectile.i.Request(barrel.transform.position);
        proj.SetOwner(false);
        proj.CreateForce(barrel.rotation);
        OnShoot?.Invoke();
    }

    void FirerateBoosterActivated(float boostValue)
    {
        isBoostActive = true;
        shootCooldown = baseShootCooldown - (boostValue * baseShootCooldown);
    }

    void FirerateBoosterDeactivated()
    {
        isBoostActive = false;
        shootCooldown = baseShootCooldown;
    }
}
