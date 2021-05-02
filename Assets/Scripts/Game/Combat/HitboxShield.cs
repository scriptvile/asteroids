using System.Collections;
using UnityEngine;

public class HitboxShield : MonoBehaviour
{
    // Inspector
    [Header("References")]
    [SerializeField] GameObject visuals;

    [Header("Settings")]
    [SerializeField] float durationOnUfoSpawn;
    [SerializeField] float durationOnDamageTake;
    [SerializeField] float durationOnAllEnemiesKilled;
    [SerializeField] float durationOnBoostPickup;
    [SerializeField] float blinkTimeBeforeEnd;

    // Privates
    bool hasBlinked;
    float shieldRemainingTime;
    Animator animator;
    public bool IsActive
    {
        get
        {
            if (shieldRemainingTime <= 0) return false;
            else return true;
        }
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void OnEnable()
    {
        PlayerHitbox.OnPlayerHit += OnPlayerHit;
        PlayerLives.OnKilled += OnPlayerDestroyed;
        WaveController.OnAllEnemiesKilled += OnAllEnemiesKilled;
        WaveController.OnUfoSpawned += OnUfoSpawned;
        Booster.OnCollected += OnBoostCollected;
    }

    void OnDisable()
    {
        PlayerHitbox.OnPlayerHit -= OnPlayerHit;
        PlayerLives.OnKilled -= OnPlayerDestroyed;
        WaveController.OnAllEnemiesKilled -= OnAllEnemiesKilled;
        WaveController.OnUfoSpawned -= OnUfoSpawned;
        Booster.OnCollected -= OnBoostCollected;
    }

    void Update()
    {
        if (shieldRemainingTime >= 0)
        {
            shieldRemainingTime -= Time.deltaTime;
            if (!hasBlinked)
            {
                if (shieldRemainingTime <= blinkTimeBeforeEnd) SetBlinked(true);
            }
            if (shieldRemainingTime <= 0) Deactivate();
        }
    }

    void SetBlinked(bool value)
    {
        hasBlinked = value;
        animator.SetBool("blink", value);
    }

    public void Activate(float duration)
    {
        if (shieldRemainingTime > duration) return;

        animator.Play("activate", 0, 0);
        SetBlinked(false);
        shieldRemainingTime = duration;
        visuals.SetActive(true);
    }

    public void Deactivate()
    {
        SetBlinked(false);
        shieldRemainingTime = 0;
        visuals.SetActive(false);
    }

    IEnumerator DeactivateEndFrame()
    {
        yield return new WaitForEndOfFrame();
        Deactivate();
    }
    void OnPlayerHit()
    {
        Activate(durationOnDamageTake);
    }

    void OnPlayerDestroyed()
    {
        StartCoroutine(DeactivateEndFrame());
    }

    void OnAllEnemiesKilled()
    {
        Activate(durationOnAllEnemiesKilled);
    }

    void OnUfoSpawned()
    {
        if (durationOnUfoSpawn > 0) Activate(durationOnUfoSpawn);
    }

    void OnBoostCollected(Booster.BoostType boostType)
    {
        if (boostType != Booster.BoostType.Shield) return;
        Activate(durationOnBoostPickup);
    }
}
