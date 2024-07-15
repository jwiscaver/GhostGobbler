using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Pacman : MonoBehaviour
{
    [Tooltip("Sprite renderer component for Pacman.")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [Tooltip("Animator component for handling animations.")]
    [SerializeField]
    private Animator animator;

    private Movement movement;
    private new Collider2D collider;

    private void Awake()
    {
        // Cache components
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponent<Movement>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleInput();
        RotatePacman();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            movement.SetDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            movement.SetDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movement.SetDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            movement.SetDirection(Vector2.right);
        }
    }

    private void RotatePacman()
    {
        // Rotate Pacman to face the movement direction
        float angle = Mathf.Atan2(movement.Direction.y, movement.Direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void ResetState()
    {
        enabled = true;
        spriteRenderer.enabled = true;
        collider.enabled = true;
        movement.ResetState();
        gameObject.SetActive(true);

        // Reset Animator to the default state
        animator.ResetTrigger("Die");
        animator.Play("Movement"); // Ensure to replace "Pacman_Idle" with your idle/default animation state name
    }

    public void DeathSequence()
    {
        enabled = false;
        movement.enabled = false;
        collider.enabled = false;
        animator.SetTrigger("Die"); // Trigger death animation
    }
}
