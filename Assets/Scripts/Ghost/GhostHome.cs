using System.Collections;
using UnityEngine;

public class GhostHome : GhostBehavior
{
    [Tooltip("Transform representing the inside position of the ghost home.")]
    [SerializeField] public Transform inside;

    [Tooltip("Transform representing the outside position of the ghost home.")]
    [SerializeField] public Transform outside;

    [Tooltip("Layer mask for detecting obstacles.")]
    [SerializeField] private LayerMask obstacleLayerMask;

    private void OnEnable()
    {
        StopAllCoroutines();
    }

    private void OnDisable()
    {
        // Check for active self to prevent error when object is destroyed
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(ExitTransition());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reverse direction everytime the ghost hits a wall to create the
        // effect of the ghost bouncing around the home
        if (enabled && ((1 << collision.gameObject.layer) & obstacleLayerMask) != 0)
        {
            ghost.movement.SetDirection(-ghost.movement.Direction);
        }
    }

    private IEnumerator ExitTransition()
    {
        // Turn off movement while we manually animate the position
        ghost.movement.SetDirection(Vector2.up, true);
        ghost.movement.rigidbody.isKinematic = true;
        ghost.movement.enabled = false;

        Vector3 position = transform.position;

        float duration = 0.5f;
        float elapsed = 0f;

        // Animate to the starting point
        while (elapsed < duration)
        {
            ghost.SetPosition(Vector3.Lerp(position, inside.position, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;

        // Animate exiting the ghost home
        while (elapsed < duration)
        {
            ghost.SetPosition(Vector3.Lerp(inside.position, outside.position, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Pick a random direction left or right and re-enable movement
        ghost.movement.SetDirection(new Vector2(Random.value < 0.5f ? -1f : 1f, 0f), true);
        ghost.movement.rigidbody.isKinematic = false;
        ghost.movement.enabled = true;
    }
}