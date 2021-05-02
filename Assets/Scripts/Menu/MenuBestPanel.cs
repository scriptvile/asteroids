using UnityEngine;
using TMPro;
using System;

public class MenuBestPanel : MonoBehaviour
{
    // Inspector
    [SerializeField] TMP_Text t_score;
    [SerializeField] TMP_Text t_time;
    [SerializeField] TMP_Text t_waveNumber;

    #region Methods

    public void UpdateBestValues()
    {
        ResultData results = Persistence.BestResult;

        if (results.Score > 0)
        {
            t_score.text = "" + results.Score;

            TimeSpan timeSpan = TimeSpan.FromSeconds(results.Time);
            t_time.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            t_waveNumber.text = "" + results.WaveRank;
        }
        else
        {
            t_score.text = "n/a";
            t_time.text = "n/a";
            t_waveNumber.text = "n/a";
        }
    }
    #endregion
}