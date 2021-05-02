using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    // Inspector
    [SerializeField] TMP_Text t_sessionScore;
    [SerializeField] TMP_Text t_bestScore;


    #region Methods

    void OnEnable()
    {
        Game.OnScoreChanged += UpdateSessionScore;
        Game.OnBestDataRefreshRequest += UpdateBestScore;
    }

    void OnDisable()
    {
        Game.OnScoreChanged -= UpdateSessionScore;
        Game.OnBestDataRefreshRequest -= UpdateBestScore;
    }

    void UpdateSessionScore(int value)
    {
        string scr = "" + value;
        if (value > Persistence.BestResult.Score) scr += "*";
        t_sessionScore.text = scr;
    }

    void UpdateBestScore()
    {
        t_bestScore.text = "" + Persistence.BestResult.Score;
    }
    #endregion
}