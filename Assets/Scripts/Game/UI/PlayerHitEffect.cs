using UnityEngine;

public class PlayerHitEffect : MonoBehaviour
{
    Animator animator;

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
}