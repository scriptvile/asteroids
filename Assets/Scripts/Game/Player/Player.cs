using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerShoot))]
[RequireComponent(typeof(PlayerLives))]
public class Player : MonoBehaviour
{
    // Inspector
    [Header("References")]
    [SerializeField] PlayerHitbox hitbox;

    // Privates
    Rigidbody2D rb;
    PlayerMovement movement;
    PlayerShoot shoot;
    PlayerLives lives;
    bool isActive;


    #region Methods

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
        movement.Init(rb);
        shoot = GetComponent<PlayerShoot>();
        lives = GetComponent<PlayerLives>();
        isActive = false;
    }

    void OnEnable()
    {
        Game.OnStateChanged += OnGameStateChanged;
    }

    void OnDisable()
    {
        Game.OnStateChanged -= OnGameStateChanged;
    }

    void Restart()
    {
        transform.position = Vector2.zero;
        transform.rotation = Quaternion.identity;
        rb.velocity = Vector2.zero;
        lives.Restart();
        hitbox.Activate();
    }

    public void HandleFixedUpdate()     // Called by Game
    {
        if (!isActive) return;
        movement.HandleFixedUpdate();
    }

    public void HandleUpdate()      // Called by Game
    {
        if (!isActive) return;

        movement.HandleUpdate();
        shoot.HandleUpdate();
    }

    public void SetActive(bool value)
    {
        isActive = value;
    }

    IEnumerator SetActiveLate(bool value)
    {
        yield return new WaitForEndOfFrame();

        SetActive(value);
    }
    #endregion

    #region Event reaction

    void OnGameStateChanged(Game.State state)
    {
        switch (state)
        {
            case Game.State.Preparation:
                Restart();
                SetActive(false);
                break;
            case Game.State.Progress:
                StartCoroutine(SetActiveLate(true));   // This is to prevent shooting when starting the game from previous state.
                break;
            case Game.State.GameOver:
                SetActive(false);
                hitbox.Deactivate();
                break;
        }
    }
    #endregion
}