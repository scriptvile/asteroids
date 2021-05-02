using UnityEngine;

public class HorizontalMovement : EnemyMovement
{
    // Enums
    enum HorizontalDirection { Right, Left }

    // Inspector
    [Header("Movement Settings")]
    [SerializeField] float yForce;
    [SerializeField] float yDuration;
    [SerializeField] float horizontalSpeed;

    [Header("Spawn Settings")]
    [SerializeField] float spawnPosPaddingY;        // Used to prevent spawning at the edges so they can be fully visible.
    
    // Privates
    HorizontalDirection horizontalDirection;
    Bounds boundsY;                   // Required to calculate spawn pos.
    Bounds boundsX;
    float timeLeftToChangeY;
    bool isMovingUp;

    void Awake()
    {
        boundsY = Game.i.PlayArea.BoundsY;
        boundsX = Game.i.PlayArea.BoundsX;
    }
    override public void Restart()
    {
        base.Restart();
        RandomizeXDirection();
        RandomizeYDirection();
        SendToStartingPosition();
        CreateHorizontalForce();
        CreateVerticalForce();
    }

    override public void Init(Rigidbody2D rb)
    {
        base.Init(rb);
    }

    void RandomizeXDirection()
    {
        int r = Random.Range(0, 2);
        if (r == 0) horizontalDirection = HorizontalDirection.Right;
        else horizontalDirection = HorizontalDirection.Left;
    }
    void RandomizeYDirection()
    {
        int r = Random.Range(0, 2);
        if (r == 0) isMovingUp = true;
        else if (r==1) isMovingUp = false;
    }

    void CreateVerticalForce()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        if (isMovingUp) rb.AddForce(new Vector2(0, yForce), ForceMode2D.Impulse);
        else rb.AddForce(new Vector2(0, -yForce), ForceMode2D.Impulse);
    }

    void SendToStartingPosition()
    {

        float y = Random.Range(boundsY.min + spawnPosPaddingY, boundsY.max - spawnPosPaddingY);
        float x = 0;
        switch (horizontalDirection)
        {
            case HorizontalDirection.Right: x = boundsX.min; break;
            case HorizontalDirection.Left: x = boundsX.max; break;
            default: Debug.LogError("Unexpected MovingDirection."); break;
        }
        rb.transform.position = new Vector2(x, y);
    }

    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
        timeLeftToChangeY -= Time.deltaTime;
        if (timeLeftToChangeY <= 0)
        {
            timeLeftToChangeY = yDuration;
            isMovingUp = !isMovingUp;
            CreateVerticalForce();
        }
    }

    void CreateHorizontalForce()
    {
        rb.velocity = Vector2.zero;
        rb.drag = 0;
        switch (horizontalDirection)
        {
            case HorizontalDirection.Left:
                rb.AddForce(Vector2.left * horizontalSpeed, ForceMode2D.Impulse);
                break;
            case HorizontalDirection.Right:
                rb.AddForce(Vector2.right * horizontalSpeed, ForceMode2D.Impulse);
                break;
            default: Debug.LogError("Unexpected MovingDirection."); return;
        }
    }
}
