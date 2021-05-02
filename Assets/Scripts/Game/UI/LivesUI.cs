using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    // Privates
    Queue<LifeUI> lifeUis = new Queue<LifeUI>();


    #region Methods

    void OnEnable()
    {
        PlayerLives.OnLivesChanged += UpdateLives;
    }

    void OnDisable()
    {
        PlayerLives.OnLivesChanged -= UpdateLives;
    }

    void UpdateLives(int value)
    {
        if (lifeUis.Count < value)
        {
            int difference = value - lifeUis.Count;
            for (int i = 0; i < difference; i++)
            {
                LifeUI lifeUi = Pool_LifeUI.i.Request();
                if (lifeUi == null)
                {
                    Debug.LogWarning("Cannot display more LifeUI's. Pool has cancelled the request.");
                    return;
                }
                else lifeUis.Enqueue(lifeUi);
            }
        } else if (lifeUis.Count > value)
        {
            int difference = lifeUis.Count - value;
            for (int i = 0; i < difference; i++)
            {
                lifeUis.Dequeue().Despawn();
            }
        }
    }
    #endregion
}