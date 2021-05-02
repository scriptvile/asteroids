using UnityEngine;
using System;

public class Booster : MonoBehaviour, IPoolable
{
    // Enums
    public enum BoostType { ScoreMultiplier, Shield, IncreaseFirerate }

    // Events
    public static event Action<BoostType> OnCollected;

    // Inspector
    [SerializeField] SpriteRenderer sr;

    // Privates
    BoostType type;

    // Properties
    public BoostType Type { get { return type; } }

    public void Spawn()
    {
        SetRandom();
    }

    public void Despawn()
    {
        Pool_Booster.i.ReturnToPool(this);
    }

    void Set(BoostType newType)
    {
        type = newType;
        switch (type)
        {
            case BoostType.ScoreMultiplier:
                sr.sprite = Pool_Booster.i.spr_scoreMultiplier;
                break;
            case BoostType.Shield:
                sr.sprite = Pool_Booster.i.spr_shield;
                break;
            case BoostType.IncreaseFirerate:
                sr.sprite = Pool_Booster.i.spr_firerate;
                break;
            default:  Debug.LogError("Unexpected BoostType."); break;
        }
    }

    void SetRandom()
    {
        int r = UnityEngine.Random.Range(0, 3);
        switch (r)
        {
            case 0: Set(Booster.BoostType.IncreaseFirerate); break;
            case 1: Set(Booster.BoostType.ScoreMultiplier); break;
            case 2: Set(Booster.BoostType.Shield); break;
            default: Debug.LogError("Unexpected Booster Type."); break;
        }
    }

    void OnTriggerStay2D(Collider2D col)        
    {
        if (col.CompareTag("Player"))
        {
            OnCollected?.Invoke(type);
            Despawn();
        }
    }
}