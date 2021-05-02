using UnityEngine;

public class PlayerMovement : WrapObject
{
    // Inspector
    [Header("References")]
    [SerializeField] GameObject engineFire;

    [Header("Settings")]
    [SerializeField] float rotationSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float pushbackFromShooting;

    // Privates
    Rigidbody2D rb;
    float inputRotationValue;
    float inputAccelerationValue;


    #region Methods

    private void OnEnable()
    {
        PlayerLives.OnKilled += OnKilled;
        PlayerShoot.OnShoot += OnShoot;
    }

    void OnDisable()
    {
        PlayerLives.OnKilled -= OnKilled;
        PlayerShoot.OnShoot -= OnShoot;
    }

    public void Init(Rigidbody2D rigidbody)
    {
        InitBounds();
        rb = rigidbody;
        engineFire.SetActive(false);

    }

    public void HandleUpdate()
    {
        inputRotationValue = Input.GetAxisRaw("Horizontal");
        inputAccelerationValue = Input.GetAxisRaw("Vertical");
    }

    public void HandleFixedUpdate()     // Runs when Player is active.
    {
        if (inputAccelerationValue > 0)
        {
            rb.AddRelativeForce(Vector2.up * acceleration * Time.deltaTime);
            engineFire.SetActive(true);
        }
        else engineFire.SetActive(false);

        if (inputRotationValue > 0)
        {
            rb.AddTorque(-rotationSpeed * Time.deltaTime);
        }
        else if (inputRotationValue < 0)
        {
            rb.AddTorque(rotationSpeed * Time.deltaTime);
        }
    }

    void FixedUpdate()      // Runs even if Player is inactive
    {
        WrapCheck();
    }
    #endregion

    #region Event reaction

    void OnKilled()
    {
        engineFire.SetActive(false);
    }

    void OnShoot()
    {
        rb.AddRelativeForce(Vector2.down * pushbackFromShooting, ForceMode2D.Impulse);
    }
    #endregion
}