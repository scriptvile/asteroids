using UnityEngine;

public class PlayerHitEffect : MonoBehaviour
{
    // Privates
    Animator animator;


    #region Methods

    void OnEnable()
    {
        PlayerLives.OnLifeLost += Trigger;
    }

    void OnDisable()
    {
        PlayerLives.OnLifeLost -= Trigger;
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Trigger()
    {
        animator.Play("trigger", 0, 0);
    }
    #endregion
}