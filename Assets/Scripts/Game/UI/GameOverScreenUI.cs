using UnityEngine;

public class GameOverScreenUI : MonoBehaviour
{
    // Inspector
    [SerializeField] GameOverRecordPanelUI recordPanel;

    public void SetBestBeaten(bool value)
    {
        recordPanel.SetPanel(value);
    }
}