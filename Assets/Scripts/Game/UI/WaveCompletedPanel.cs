using UnityEngine;

public class WaveCompletedPanel : MonoBehaviour
{
    // Privates
    Animator animator;


    #region Methods

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        WaveController.OnAllEnemiesKilled += Display;
    }

    void OnDisable()
    {
        WaveController.OnAllEnemiesKilled -= Display;
    }

    void Display()
    {
        animator.Play("show", 0, 0);
    }
    #endregion
}