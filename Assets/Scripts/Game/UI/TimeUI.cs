using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class TimeUI : MonoBehaviour
{
    // Inspector
    [SerializeField] TMP_Text t_time;

    // Privates
    TimeSpan timeSpan;
    bool isPaused;


    #region Methods

    void OnEnable()
    {
        Game.OnStateChanged += OnGameStateChanged;
    }

    void OnDisable()
    {
        Game.OnStateChanged -= OnGameStateChanged;
    }

    void Awake()
    {
        Pause();
    }

    void Update()
    {
        if (isPaused) return;
        RefreshTime();
    }

    void Pause()
    {
        isPaused = true;
    }

    void Unpause()
    {
        isPaused = false;
    }

    void RefreshTime()
    {
        timeSpan = TimeSpan.FromSeconds(Session.PlayTime);
        t_time.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    IEnumerator RefreshTimeLate()
    {
        yield return new WaitForEndOfFrame();
        RefreshTime();
    }
    #endregion

    #region Event reaction
    void OnGameStateChanged(Game.State newState)
    {
        if (newState == Game.State.Progress) Unpause();
        else
        {
            StartCoroutine(RefreshTimeLate());
            Pause();
        }
    }
    #endregion
}