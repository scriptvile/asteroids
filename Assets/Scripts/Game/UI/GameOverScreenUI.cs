using UnityEngine;

public class GameOverScreenUI : MonoBehaviour
{
    // Inspector
    [SerializeField] GameOverRecordPanelUI recordPanel;


    #region Methods

    public void SetBestBeaten(bool value)
    {
        recordPanel.SetPanel(value);
    }
    #endregion
}