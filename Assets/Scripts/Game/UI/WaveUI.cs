using UnityEngine;
using TMPro;

public class WaveUI : MonoBehaviour
{
    // Inspector
    [SerializeField] TMP_Text t_wave;

    #region Methods

    void OnEnable()
    {
        WaveController.OnWaveRankChanged += UpdateWaveNumber;
    }

    void OnDisable()
    {
        WaveController.OnWaveRankChanged -= UpdateWaveNumber;
    }

    public void UpdateWaveNumber(int waveNumber)
    {
        if (waveNumber <= 0) t_wave.text = "0";
        else t_wave.text = "" + waveNumber;
    }
    #endregion
}