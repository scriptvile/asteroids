using UnityEngine;

public class EnemyShoot : Shoot
{
    public void Restart()
    {
        timeToNextShoot = shootCooldown;
    }

    override public void HandleUpdate()
    {
        base.HandleUpdate();
        if (timeToNextShoot <= 0) ShootProjectile();
    }

    override protected void ShootProjectile()
    {
        base.ShootProjectile();
        Projectile proj = Pool_Projectile.i.Request(barrel.transform.position);
        proj.SetOwner(true);
        proj.CreateForce(barrel.rotation);
        barrel.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360f));
    }
}