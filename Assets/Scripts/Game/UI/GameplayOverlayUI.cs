using UnityEngine;

public class GameplayOverlayUI : MonoBehaviour
{
    // Inspector
    [Header("References")]
    [SerializeField] GameObject preparationScreen;
    [SerializeField] GameOverScreenUI gameOverScreen;
    [SerializeField] GameObject pauseOverlay;
    [SerializeField] PlayerHitEffect playerHitEffect;


    #region Methods

    void Awake()
    {
        playerHitEffect.gameObject.SetActive(true);
        HidePauseOverlay();
    }

    void OnEnable()
    {
        Game.OnStateChanged += OnGameStateChanged;
        Game.OnPaused += ShowPauseOverlay;
        Game.OnResumed += HidePauseOverlay;
    }

    void OnDisable()
    {
        Game.OnStateChanged -= OnGameStateChanged;
        Game.OnPaused -= ShowPauseOverlay;
        Game.OnResumed -= HidePauseOverlay;
    }

    void OnGameStateChanged(Game.State gameState)
    {
        switch (gameState)
        {
            case Game.State.Preparation:
                preparationScreen.SetActive(true);
                gameOverScreen.gameObject.SetActive(false);
                break;
            case Game.State.Progress:
                preparationScreen.SetActive(false);
                gameOverScreen.gameObject.SetActive(false);
                break;
            case Game.State.GameOver:
                preparationScreen.SetActive(false);
                gameOverScreen.gameObject.SetActive(true);
                if (Session.Score > Persistence.BestResult.Score)gameOverScreen.SetBestBeaten(true);
                else gameOverScreen.SetBestBeaten(false);
                break;
            default: Debug.LogError("Unexpected Game State."); return;
               
        }
    }

    void ShowPauseOverlay()
    {
        pauseOverlay.SetActive(true);
    }

    void HidePauseOverlay()
    {
        pauseOverlay.SetActive(false);
    }
    #endregion
}