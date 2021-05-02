using UnityEngine;
using System;

public class PlayerLives : MonoBehaviour
{
    // Events
    public static event Action<int> OnLivesChanged;
    public static event Action OnKilled;
    public static event Action OnLifeLost;

    // Inspector
    [SerializeField] int startingLives;
    [SerializeField] int scoreToLifeUp;

    // Privates
    int currentLives;
    int remainingScoreToLifeUp;


    #region Methods

    void OnEnable()
    {
        PlayerHitbox.OnPlayerHit += OnPlayerHit;
        Game.OnScoreAdded += OnScoreAdded;
    }

    void OnDisable()
    {
        PlayerHitbox.OnPlayerHit -= OnPlayerHit;
        Game.OnScoreAdded -= OnScoreAdded;
    }

    public void Restart()
    {
        Set(startingLives);
        remainingScoreToLifeUp = scoreToLifeUp;
    }

    void Set(int value)
    {
        if (value < 0) value = 0;
        if (currentLives != value)
        {
            currentLives = value;
            OnLivesChanged?.Invoke(value);
        }
    }

    void Add()
    {
        Set(currentLives + 1);
    }

    void Subtract()
    {
        if (currentLives == 0) OnKilled?.Invoke();
        Set(currentLives - 1);
        OnLifeLost?.Invoke();
    }
    #endregion

    #region Event reaction

    void OnPlayerHit()
    {
        Subtract();
    }

    void OnScoreAdded(int value)
    {
        remainingScoreToLifeUp -= value;

        if (remainingScoreToLifeUp <= 0)
        {
            remainingScoreToLifeUp = (scoreToLifeUp + remainingScoreToLifeUp);
            Add();

            FloatingText t = Pool<FloatingText>.i.Request(Game.i.Player.transform.position);
            t.SetText("Life Up!", new Color(0.2f, 1, 0), FloatingText.Size.Normal);
        }
    }
    #endregion
}

