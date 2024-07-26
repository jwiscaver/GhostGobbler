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

        animator.ResetTrigger("Die");
        animator.Play("Movement");
    }

    public void DeathSequence()
    {
        enabled = false;
        movement.enabled = false;
        collider.enabled = false;
        animator.SetTrigger("Die");
    }
}