using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class GhostMove : MonoBehaviour
{
    [Tooltip("The parent GameObject that holds all waypoint children.")]
    public Transform waypointsParent;
    private Transform[] waypoints;
    private int curWaypointIndex = 0;
    [Tooltip("Movement speed of the ghost.")]
    public float speed = 3f;

    private Rigidbody2D rb2D;
    private Animator animator;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (waypointsParent != null)
        {
            waypoints = new Transform[waypointsParent.childCount];
            for (int i = 0; i < waypointsParent.childCount; i++)
            {
                waypoints[i] = waypointsParent.GetChild(i);
            }
        }
        else
        {
            Debug.LogError("Waypoints parent not set on " + gameObject.name);
        }
    }

    void FixedUpdate()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            return;
        }

        if (Vector2.Distance(transform.position, waypoints[curWaypointIndex].position) > 0.1f)
        {
            Vector2 newPosition = Vector2.MoveTowards(transform.position, waypoints[curWaypointIndex].position, speed * Time.fixedDeltaTime);
            rb2D.MovePosition(newPosition);
        }
        else
        {
            curWaypointIndex = (curWaypointIndex + 1) % waypoints.Length;
        }

        Vector2 direction = waypoints[curWaypointIndex].position - transform.position;
        animator.SetFloat("DirX", direction.x);
        animator.SetFloat("DirY", direction.y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }
    }
}