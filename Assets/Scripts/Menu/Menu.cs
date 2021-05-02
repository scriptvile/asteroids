using UnityEngine;

public class Menu : MonoBehaviour
{
    // Inspector
    [SerializeField] MenuBestPanel bestPanel;

    #region Methods

    void Start()
    {
        bestPanel.UpdateBestValues();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) Main.i.ChangeScene(Scene.Game);
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }
    #endregion
}