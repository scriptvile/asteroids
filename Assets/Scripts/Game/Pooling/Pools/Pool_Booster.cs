using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Pool_Booster : Pool<Booster>
{
    // Events
    public static event Action<float> OnFirerateBoostActivated;
    public static event Action OnFirerateBoostDeactivated;

    // Singleton
    new public static Pool_Booster i;

    // Inspector
    [Header("References")]
    [SerializeField] BoosterEffectUIController boosterUiController;

    [Header("Settings")]
    [SerializeField] int scoreToSpawnBooster;

    [Header("Booster sprite references")]
    public Sprite spr_firerate;
    public Sprite spr_shield;
    public Sprite spr_scoreMultiplier;

    [Header("Score Multiplier Boost Settings")]
    [SerializeField] float scoreBoostDuration;
    [SerializeField] int scoreBoostMultiplier;

    [Header("Firerate Boost settings")]
    [SerializeField] float firerateBoostDuration;
    [SerializeField] float firerateBoostAmount;

    // Privates
    float scoreBoostRemainingTime;
    float firerateBoostRemainingTime;
    int scoreToNextBooster;
    
    // Properties
    public bool IsScoreBoostActive
    {
        get
        {
            if (scoreBoostRemainingTime > 0) return true;
            else return false;
        }
    }
    public int ScoreBoostMultiplier { get { return scoreBoostMultiplier; } }

    void Awake()
    {
        if (i == null) i = this;
        else if (i != this) Destroy(gameObject);
    }

    override protected void OnEnable()
    {
        base.OnEnable();
        Booster.OnCollected += OnBoosterCollected;
        Game.OnScoreAdded += OnScoreAdded;
    }

    override protected void OnDisable()
    {
        base.OnDisable();
        Booster.OnCollected -= OnBoosterCollected;
        Game.OnScoreAdded -= OnScoreAdded;
    }

    public void Restart()
    {
        DeactivateFirerateBoost();
        DeactivateScoreBoost();
        scoreToNextBooster = scoreToSpawnBooster;
    }

    void Update()
    {
        if (scoreBoostRemainingTime > 0)
        {
            scoreBoostRemainingTime -= Time.deltaTime;
            if (scoreBoostRemainingTime <= 0) DeactivateScoreBoost();
        }

        if (firerateBoostRemainingTime > 0)
        {
            firerateBoostRemainingTime -= Time.deltaTime;
            if (firerateBoostRemainingTime <= 0) DeactivateFirerateBoost();
        }
    }

    void SpawnRandomBooster()
    {
        Request(new Vector2(Bounds.GetRandomFloat(Game.i.PlayArea.BoundsX), Bounds.GetRandomFloat(Game.i.PlayArea.BoundsY)));
    }

    void ActivateScoreBoost()
    {
        boosterUiController.Display("yellow");
        scoreBoostRemainingTime = scoreBoostDuration;
    }

     void DeactivateScoreBoost()
    {
        boosterUiController.Hide("yellow");
        scoreBoostRemainingTime = 0;
    }

    void ActivateFirerateBoost()
    {
        boosterUiController.Display("red");
        firerateBoostRemainingTime = firerateBoostDuration;
        OnFirerateBoostActivated?.Invoke(firerateBoostAmount);
    }

     void DeactivateFirerateBoost()
    {
        boosterUiController.Hide("red");
        firerateBoostRemainingTime = 0;
        OnFirerateBoostDeactivated?.Invoke();
    }

    void OnBoosterCollected(Booster.BoostType type)
    {
        switch (type)
        {
            case Booster.BoostType.ScoreMultiplier: ActivateScoreBoost(); break;
            case Booster.BoostType.IncreaseFirerate: ActivateFirerateBoost(); break;
            case Booster.BoostType.Shield: // Leave empty. Shield only needs to be activated.
                break;
            default: Debug.LogError("Unexpected BoostType."); break;
        }
    }

    void OnScoreAdded(int value)
    {
        scoreToNextBooster -= value;

        if (scoreToNextBooster <= 0)
        {
            scoreToNextBooster = (scoreToSpawnBooster + scoreToNextBooster);
            SpawnRandomBooster();
        }
    }

}