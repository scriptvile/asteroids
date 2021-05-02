using UnityEngine;

public class PauseUI : MonoBehaviour
{
    // Inspector
    [Header("References")]
    [SerializeField] GameObject pausedPanel;
    [SerializeField] GameObject unpausedPanel;

    void OnEnable()
    {
        Game.OnPaused += SetPaused;
        Game.OnResumed += SetUnpaused;
    }

    void OnDisable()
    {
        Game.OnPaused -= SetPaused;
        Game.OnResumed -= SetUnpaused;
    }

    void SetPaused()
    {
        pausedPanel.SetActive(true);
        unpausedPanel.SetActive(false);
    }

    void SetUnpaused()
    {
        pausedPanel.SetActive(false);
        unpausedPanel.SetActive(true);
    }
}
