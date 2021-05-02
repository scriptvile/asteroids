using UnityEngine;

public class GameOverRecordPanelUI : MonoBehaviour
{
    // Inspector
    [SerializeField] GameObject noRecord;
    [SerializeField] GameObject newRecord;

    public void SetPanel(bool recordBeaten)
    {
        if (recordBeaten)
        {
            noRecord.SetActive(false);
            newRecord.SetActive(true);
        }
        else
        {
            noRecord.SetActive(true);
            newRecord.SetActive(false);
        }
    }
}