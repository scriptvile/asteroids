using UnityEngine;

public abstract class Shoot : MonoBehaviour
{
    // Inspector
    [SerializeField] protected Transform barrel;
    [SerializeField] protected float shootCooldown;

    // Privates
    protected float timeToNextShoot;


    #region Methods

    virtual public void HandleUpdate()
    {
        if (timeToNextShoot > 0) timeToNextShoot -= Time.deltaTime;
    }

    virtual protected void ShootProjectile()
    {
        timeToNextShoot = shootCooldown;
    }
    #endregion
}