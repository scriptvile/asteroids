using System.Collections.Generic;
using UnityEngine;

public class BoosterEffectUIController : MonoBehaviour
{
    // Inspector
    [SerializeField] List<BoosterEffectElement> boosterEffects = new List<BoosterEffectElement>();
    [SerializeField] GameObject root;


    #region Methods

    void Awake()
    {
        for (int i = 0; i < boosterEffects.Count; i++)          // Make sure all gameObjects are active.
        {
            boosterEffects[i].obj.gameObject.SetActive(true);
        }
    }

    public void Display(string id)
    {
        BoosterEffectUI obj = GetObj(id);
        if (obj == null) return;

        obj.Show();
    }

    public void Hide(string id)
    {
        BoosterEffectUI obj = GetObj(id);
        if (obj == null) return;

        obj.Hide();
    }

    BoosterEffectUI GetObj(string id)
    {
        for (int i = 0; i < boosterEffects.Count; i++)
        {
            if (boosterEffects[i].id == id)
            {
                return boosterEffects[i].obj;
            }
        }

        Debug.LogError("Incorrect ID.");
        return null;
    }
    #endregion
}