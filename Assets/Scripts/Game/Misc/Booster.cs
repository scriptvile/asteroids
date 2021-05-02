using UnityEngine;
using System;

public class Booster : MonoBehaviour, IPoolable
{
    // Enums
    public enum Type { ScoreMultiplier, Shield, IncreaseFirerate }

    // Events
    public static event Action<Type> OnCollected;

    // Inspector
    [SerializeField] SpriteRenderer sr;

    // Privates
    Type type;


    #region Methods

    public void Spawn()
    {
        SetRandom();
    }

    public void Despawn()
    {
        Pool_Booster.i.ReturnToPool(this);
    }

    void Set(Type newType)
    {
        type = newType;
        switch (type)
        {
            case Type.ScoreMultiplier:
                sr.sprite = Pool_Booster.i.spr_scoreMultiplier;
                break;
            case Type.Shield:
                sr.sprite = Pool_Booster.i.spr_shield;
                break;
            case Type.IncreaseFirerate:
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
            case 0: Set(Booster.Type.IncreaseFirerate); break;
            case 1: Set(Booster.Type.ScoreMultiplier); break;
            case 2: Set(Booster.Type.Shield); break;
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
    #endregion
}