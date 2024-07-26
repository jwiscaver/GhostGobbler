using UnityEngine;

[DefaultExecutionOrder(-10)]
[RequireComponent(typeof(GhostMovement))]
[RequireComponent(typeof(GhostHome))]
[RequireComponent(typeof(GhostScatter))]
[RequireComponent(typeof(GhostChase))]
[RequireComponent(typeof(GhostFrightened))]
public class Ghost : MonoBehaviour
{
    [Tooltip("Initial behavior for the ghost.")]
    [SerializeField] private GhostBehavior initialBehavior;

    [Tooltip("Layer mask for detecting Pacman.")]
    [SerializeField] private LayerMask pacmanLayerMask;

    [Tooltip("Points awarded for eating the ghost.")]
    [SerializeField] private int points = 200;

    [Tooltip("Target transform the ghost is chasing.")]
    [SerializeField] private Transform target;

    public int Points => points;
    public Transform Target => target;

    public GhostMovement movement { get; private set; }
    public GhostHome home { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostChase chase { get; private set; }
    public GhostFrightened frightened { get; private set; }

    private void Awake()
    {
        movement = GetComponent<GhostMovement>();
        home = GetComponent<GhostHome>();
        scatter = GetComponent<GhostScatter>();
        chase = GetComponent<GhostChase>();
        frightened = GetComponent<GhostFrightened>();
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        gameObject.SetActive(true);
        movement.ResetState();

        frightened.Disable();
        chase.Disable();
        scatter.Enable();

        if (home != initialBehavior)
        {
            home.Disable();
        }

        if (initialBehavior != null)
        {
            initialBehavior.Enable();
        }
    }

    public void SetPosition(Vector3 position)
    {
        position.z = transform.position.z;
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & pacmanLayerMask) != 0)
        {
            if (frightened.enabled)
            {
                GameManager.Instance.GhostEaten(this);
            }
            else
            {
                GameManager.Instance.PacmanEaten();
            }
        }
    }
}
