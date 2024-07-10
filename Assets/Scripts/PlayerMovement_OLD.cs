using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Movement speed of the player.")]
    public float speed = 0.4f;
    private Vector2 destination;
    private float tolerance = 0.05f;

    private Rigidbody2D rb2D;
    private Animator animator;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        destination = transform.position;
    }

    void Start()
    {
        destination = transform.position;
    }

    void FixedUpdate()
    {
        MovePlayer();

        if (IsAtDestination())
        {
            UpdateDestination();
        }

        UpdateAnimationParameters();
    }

    private void MovePlayer()
    {
        Vector2 newPosition = Vector2.MoveTowards(rb2D.position, destination, speed * Time.fixedDeltaTime);
        rb2D.MovePosition(newPosition);
    }

    private bool IsAtDestination()
    {
        bool atDestination = Vector2.Distance(rb2D.position, destination) < tolerance;
        if (atDestination)
        {
            Debug.Log($"At destination: {destination}");
        }
        return atDestination;
    }

    private void UpdateDestination()
    {
        Vector2 currentPos = rb2D.position;
        if (Input.GetKey(KeyCode.UpArrow) && IsValidMove(Vector2.up))
        {
            destination = currentPos + Vector2.up;
        }
        else if (Input.GetKey(KeyCode.RightArrow) && IsValidMove(Vector2.right))
        {
            destination = currentPos + Vector2.right;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && IsValidMove(Vector2.down))
        {
            destination = currentPos + Vector2.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && IsValidMove(Vector2.left))
        {
            destination = currentPos + Vector2.left;
        }

        Debug.Log($"New destination set: {destination}");
    }

    private void UpdateAnimationParameters()
    {
        Vector2 direction = destination - rb2D.position;
        animator.SetFloat("DirX", direction.x);
        animator.SetFloat("DirY", direction.y);
    }

    private bool IsValidMove(Vector2 direction)
    {
        /*Vector2 position = rb2D.position;
        RaycastHit2D hit = Physics2D.Linecast(position + direction, position);
        Debug.Log($"Checking move in direction {direction}. Hit: {hit.collider?.name}");

        return hit.collider == null || !hit.collider.CompareTag("Wall");
        */
        // cast line from 'next to pacman' to pacman
        // not from directly the center of next tile but just a little further from center of next tile
        Vector2 pos = transform.position;
        direction += new Vector2(direction.x * 0.45f, direction.y * 0.45f);
        RaycastHit2D hit = Physics2D.Linecast(pos + direction, pos);
        return (hit.collider == GetComponent<Collider2D>());
    }
}