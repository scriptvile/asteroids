using UnityEngine;

public class WaveCompletedPanel : MonoBehaviour
{
    // Privates
    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void OnEnable()
    {
        WaveController.OnAllEnemiesKilled += OnWaveCompleted;
    }

    void OnDisable()
    {
        WaveController.OnAllEnemiesKilled -= OnWaveCompleted;
    }

    void OnWaveCompleted()
    {
        animator.Play("show", 0, 0);
    }
}