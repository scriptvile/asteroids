using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Game : MonoBehaviour
{
    // Enums
    public enum State { Preparation, Progress, GameOver }

    // Events
    public static event Action<State> OnStateChanged;
    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnScoreAdded;
    public static event Action OnPaused;
    public static event Action OnResumed;
    public static event Action OnBestDataRefreshRequest;
    void BestDataRefreshRequest() => OnBestDataRefreshRequest?.Invoke();

    // Singleton
    public static Game i = null;

    // Inspector
    [Header("References")]
    [SerializeField] PlayArea playArea;
    [SerializeField] Player player;

    // Privates
    bool isPaused = false;
    State state;

    // Properties
    public PlayArea PlayArea { get { return playArea; } }
    public Player Player { get { return player; } }


    #region Methods

    void Awake()
    {
        if (i == null) i = this;
        else if (i != this)
        {
            Debug.LogError("Unexpected singleton duplicate (Level).");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SetState(State.Preparation);
    }

    void OnEnable()
    {
        PlayerLives.OnKilled += OnPlayerDestroyed;
        Enemy.OnPlayerKilledEnemy += OnPlayerKilledEnemy;
    }

    void OnDisable()
    {
        PlayerLives.OnKilled -= OnPlayerDestroyed;
        Enemy.OnPlayerKilledEnemy -= OnPlayerKilledEnemy;
    }

    public void SpawnEnemiesAtPositions(Enemy.Type enemyType, int count, Queue<Vector2> positions)
    {
        switch (enemyType)
        {
            case Enemy.Type.AsteroidBig: Pool_AsteroidBig.i.Request(count, positions); break;
            case Enemy.Type.AsteroidMedium: Pool_AsteroidMedium.i.Request(count, positions); break;
            case Enemy.Type.AsteroidSmall: Pool_AsteroidSmall.i.Request(count, positions); break;
            default: Debug.LogError("Unexpected enemy type."); break;
        }
    }

    public void SpawnEnemiesAtRandomPositions(Enemy.Type enemyType, int count)
    {
        Queue<Vector2> spawnPositions = new Queue<Vector2>();
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = new Vector2(Bounds.GetRandomPoint(playArea.BoundsX), Bounds.GetRandomPoint(playArea.BoundsY));
            spawnPositions.Enqueue(pos);
        }

        SpawnEnemiesAtPositions(enemyType, count, spawnPositions);
    }

    public void SpawnEnemiesAtEdges(Enemy.Type enemyType, int count)
    {
        Queue<Vector2> spawnPositions = new Queue<Vector2>();
        for (int i = 0; i < count; i++)
        {
            spawnPositions.Enqueue(playArea.Edges.GetRandomPoint());
        }

        SpawnEnemiesAtPositions(enemyType, count, spawnPositions);
    }

    void Update()
    {
        switch (state)
        {
            case State.Preparation:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) SetState(State.Progress);
                if (Input.GetKeyDown(KeyCode.Escape)) Main.i.ChangeScene(Scene.Menu);
                break;
            case State.Progress:
                if (Input.GetKeyDown(KeyCode.P)) SetPaused(!isPaused);
                if (!isPaused)
                {
                    Session.PlayTime += Time.deltaTime;
                    player.HandleUpdate();
                    if (Input.GetKeyDown(KeyCode.Escape)) SetState(State.GameOver);
                } else if (Input.GetKeyDown(KeyCode.Escape)) SetPaused(false);
                break;
            case State.GameOver:
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) SetState(State.Preparation);
                if (Input.GetKeyDown(KeyCode.Escape)) Main.i.ChangeScene(Scene.Menu);
                break;
        }
    }

    void FixedUpdate()
    {
        if (state == State.Progress)
        {
            if (isPaused) return;
            player.HandleFixedUpdate();
        }
    }

    public void SetState(State newState)
    {
        state = newState;
        if (isPaused) SetPaused(false);

        switch (state)
        {
            case State.Preparation:
                BestDataRefreshRequest();
                Session.PlayTime = 0;
                SetSessionScore(0);
                Pool_Booster.i.Restart();
                break;
            case State.Progress:
                break;
            case State.GameOver:
                if (Session.Score > Persistence.BestResult.Score) StartCoroutine(SaveBestScoreLate());      // Save at the end of frame, so that gameOver panel can still access the old record.
                break;
        }

        OnStateChanged?.Invoke(state);
    }

    public void SetPaused(bool value)
    {
        isPaused = value;
        if (isPaused) Time.timeScale = 0;
        else Time.timeScale = 1;

        if (isPaused) OnPaused?.Invoke();
        else OnResumed?.Invoke();
    }

    void SetSessionScore(int value)
    {
        if (value < 0) value = 0;

        Session.Score = value;
        OnScoreChanged?.Invoke(value);
    }

    void AddSessionScore(int value)
    {
        SetSessionScore(Session.Score + value);
        OnScoreAdded?.Invoke(value);
    }

    IEnumerator SaveBestScoreLate()
    {
        yield return new WaitForEndOfFrame();
        Persistence.SaveNewResult(new ResultData(Session.Score, Session.PlayTime, Session.WaveRank));
    }
    #endregion

    #region Event reaction

    void OnPlayerDestroyed()
    {
        SetState(State.GameOver);
    }

    void OnPlayerKilledEnemy(Enemy enemy)
    {
        int score = enemy.ScoreValue;
        string scoreStr = "+" + score;

        if (Pool_Booster.i.IsScoreBoostActive)
        {
            score = score * Pool_Booster.i.ScoreBoostMultiplier;
            scoreStr = "+" + score + "*";
        }

        AddSessionScore(score);
        FloatingText text = Pool<FloatingText>.i.Request(enemy.transform.position);
        text.SetText(scoreStr, new Color(1f, 0.8f, 0), FloatingText.Size.Normal);
    }
    #endregion
}