using UnityEngine;

[DefaultExecutionOrder(-10)]
[RequireComponent(typeof(GhostMovement))]
public class Ghost : MonoBehaviour
{
    [Tooltip("Layer mask for detecting Pacman.")]
    [SerializeField] private LayerMask pacmanLayer;

    [Tooltip("Points obtained by eating ghosts.")]
    [SerializeField] private int points = 200;

    public GhostMovement movement { get; private set; }
    public GhostHome home { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostChase chase { get; private set; }
    public GhostFrightened frightened { get; private set; }
    public GhostBehavior initialBehavior;
    public Transform target;

    public int Points => points;

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
        if (((1 << collision.gameObject.layer) & pacmanLayer) != 0)
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
