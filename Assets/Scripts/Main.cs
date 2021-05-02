using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public static Main i = null;

    void Awake()
    {
        if (i == null)
        {
            i = this;
            DontDestroyOnLoad(this);
            Debug.Log(Application.persistentDataPath);
            LoadPersistentData();
        }
        else if (i != this) Destroy(gameObject);
    }

    public void ChangeScene(Scene scene)
    {
        switch (scene)
        {
            case Scene.Game: SceneManager.LoadScene("Game"); break;
            case Scene.Menu: SceneManager.LoadScene("Menu"); break;
            default: Debug.LogError("Unexpected scene."); break;
        }
    }

    void LoadPersistentData()
    {
        Persistence.LoadFromFile();
    }

}