using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [Tooltip("Movement speed of the player.")]
    [SerializeField] public float speed = 8f;

    [Tooltip("Speed multiplier for the player.")]
    [SerializeField] public float speedMultiplier = 1f;

    [Tooltip("Initial direction of movement.")]
    [SerializeField] public Vector2 initialDirection;

    [Tooltip("Layer mask to detect obstacles.")]
    [SerializeField] public LayerMask obstacleLayer;

    [Tooltip("Size of the BoxCast for movement validation.")]
    [SerializeField] private Vector2 boxCastSize = Vector2.one * 0.75f;

    [Tooltip("Distance of the BoxCast for movement validation.")]
    [SerializeField] private float boxCastDistance = 1.5f;

    public new Rigidbody2D rigidbody { get; private set; }
    public Vector2 direction { get; private set; }
    public Vector2 nextDirection { get; private set; }
    public Vector3 startingPosition { get; private set; }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        speedMultiplier = 1f;
        direction = initialDirection;
        nextDirection = Vector2.zero;
        transform.position = startingPosition;
        rigidbody.isKinematic = false;
        enabled = true;
    }

    private void Update()
    {
        if (nextDirection != Vector2.zero)
        {
            SetDirection(nextDirection);
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        Vector2 translation = speed * speedMultiplier * Time.fixedDeltaTime * direction;

        rigidbody.MovePosition(position + translation);
    }

    public void SetDirection(Vector2 direction, bool forced = false)
    {
        if (forced || IsValidMove(direction))
        {
            this.direction = direction;
            nextDirection = Vector2.zero;
        }
        else
        {
            nextDirection = direction;
        }
    }

    public bool IsValidMove(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxCastSize, 0f, direction, boxCastDistance, obstacleLayer);
        return hit.collider == null;
    }
}