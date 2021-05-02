using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class WaveController : MonoBehaviour
{
    // Enums
    public enum State { WaitAfterWave, Progress, Idle }

    // Events
    public static event Action OnAllEnemiesKilled;
    public static event Action<int> OnWaveRankChanged;
    public static event Action OnUfoSpawned;

    // Inspector
    [Header("Waves settings")]
    [SerializeField] float waveDelay;

    [Header("Spawn Asteroids settings")]
    [SerializeField] bool alwaysSpawnOnEdges;
    [SerializeField] int asteroidCountOnStart;
    [SerializeField] int asteroidsPerWave;

    [Header("Spawn UFO settings")]
    [SerializeField] bool enableSpawningUfos;
    [SerializeField] bool ufoAppearsOnFirstWave;
    [SerializeField] int ufosPerWave;

    // Privates
    HashSet<Enemy> enemies = new HashSet<Enemy>();       // Enemies required to kill in order to trigger the next wave.
    int currentWave;
    State state;
    float waitTimeLeft;                 // Applicable only in WaitAfterWave state.
    int ufosToSpawnThisWave;
    int ufoSpawnFrequency;
    int nextUfoSpawn;
    int ufosSpawnedThisWave;
    int mediumAsteroidSpawnCount;
    int smallAsteroidsSpawnCount;

    // Properties
    int BigAsteroidCountThisWave { get { return asteroidCountOnStart + (asteroidsPerWave * (currentWave - 1)); } }

    void OnEnable()
    {
        Enemy.OnEnemySpawned += OnEnemySpawned;
        Enemy.OnEnemyDespawned += OnEnemyDespawned;
        Enemy.OnPlayerKilledEnemy += OnEnemyKilled;
        Game.OnStateChanged += OnGameStateChanged;
    }

    void OnDisable()
    {
        Enemy.OnEnemySpawned -= OnEnemySpawned;
        Enemy.OnEnemyDespawned -= OnEnemyDespawned;
        Enemy.OnPlayerKilledEnemy -= OnEnemyKilled;
        Game.OnStateChanged -= OnGameStateChanged;
    }

    void Start()
    {
        mediumAsteroidSpawnCount = Pool_AsteroidBig.i.Prefab.SpawnCountOnKill;
        smallAsteroidsSpawnCount = Pool_AsteroidMedium.i.Prefab.SpawnCountOnKill;
    }

    void Update()
    {
        if (state != State.WaitAfterWave) return;

        waitTimeLeft -= Time.deltaTime;
        if (waitTimeLeft <= 0)
        {
            IncrementWave();
            SetState(State.Progress);
        }
    }

    void RestartWaves()
    {
        SetWaveRank(0);
    }

    public void SetState(State newState)
    {
        state = newState;

        if (state == State.WaitAfterWave) waitTimeLeft = waveDelay;
    }

    public void SetWaveRank(int waveNumber)
    {
        void CalculateUFOSpawns()
        {
            ufosToSpawnThisWave = currentWave * ufosPerWave;
            if (!ufoAppearsOnFirstWave) ufosToSpawnThisWave -= ufosPerWave;
            int mediumAsteroidCountThisWave = BigAsteroidCountThisWave * mediumAsteroidSpawnCount;
            int smallAsteroidCountThisWave = mediumAsteroidCountThisWave * smallAsteroidsSpawnCount;
            int totalAsteroidCountThisWave = BigAsteroidCountThisWave + mediumAsteroidCountThisWave + smallAsteroidCountThisWave;
            ufosSpawnedThisWave = 0;

            if (ufosToSpawnThisWave > 0)
            {
                float frequency = (float)totalAsteroidCountThisWave / (float)ufosToSpawnThisWave;
                ufoSpawnFrequency = (int)frequency;
                nextUfoSpawn = ufoSpawnFrequency;
            }
        }

        ClearEnemies();
        if (waveNumber <= 0)
        {
            currentWave = 0;
            OnWaveRankChanged?.Invoke(currentWave);
            return;
        }

        currentWave = waveNumber;
        Session.WaveRank = currentWave;
        CalculateUFOSpawns();

        if (alwaysSpawnOnEdges) Game.i.SpawnEnemiesAtEdges(Enemy.Type.AsteroidBig, BigAsteroidCountThisWave);
        else
        {
            if (currentWave == 1) Game.i.SpawnEnemiesAtEdges(Enemy.Type.AsteroidBig, BigAsteroidCountThisWave);
            else Game.i.SpawnEnemiesAtRandomPositions(Enemy.Type.AsteroidBig, BigAsteroidCountThisWave);
        }

        OnWaveRankChanged?.Invoke(currentWave);
    }

    public void IncrementWave()
    {
        currentWave++;
        SetWaveRank(currentWave);
    }

    public void ClearEnemies()
    {
        foreach (Enemy e in enemies)
        {
            e.MarkForDespawn();
        }
    }

    void OnEnemySpawned(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    void OnEnemyDespawned(Enemy enemy)
    {
        enemies.Remove(enemy);
        if (state != State.Progress) return;
        StartCoroutine(KillCheck());
    }

    IEnumerator KillCheck()
    {
        yield return new WaitForEndOfFrame();

        if (enemies.Count == 0)
        {
            OnAllEnemiesKilled?.Invoke();
            SetState(State.WaitAfterWave);
        }
    }

    void OnGameStateChanged(Game.State gameState)
    {
        switch (gameState)
        {
            case Game.State.Preparation:
                RestartWaves();
                SetState(State.Idle);
                break;
            case Game.State.Progress:
                SetState(State.Progress);
                IncrementWave();
                break;
            case Game.State.GameOver:
                SetState(State.Idle);
                break;
        }
    }

    void OnEnemyKilled(Enemy e)
    {
        if (enableSpawningUfos && e is Asteroid)
        {
            if (ufosToSpawnThisWave > 0 && ufosSpawnedThisWave < ufosToSpawnThisWave)
            {
                nextUfoSpawn -= 1;
                if (nextUfoSpawn <= 0)
                {
                    Pool<UFO>.i.Request();
                    nextUfoSpawn = ufoSpawnFrequency;
                    ufosSpawnedThisWave += 1;
                    OnUfoSpawned?.Invoke();

                    FloatingText t = Pool<FloatingText>.i.Request(Game.i.Player.transform.position + new Vector3(0, 0.5f, 0));
                    t.SetText("[Warning: UFO]", (new Color(0, 1f, 0.5f)), FloatingText.Size.Small);
                }
            }
        }
    }
}
