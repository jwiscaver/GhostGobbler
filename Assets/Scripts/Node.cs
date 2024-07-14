using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [Tooltip("Layer mask for detecting Obstacle.")]
    [SerializeField] private LayerMask obstacleLayer;
    public readonly List<Vector2> availableDirections = new();

    private void Start()
    {
        availableDirections.Clear();

        CheckAvailableDirection(Vector2.up);
        CheckAvailableDirection(Vector2.down);
        CheckAvailableDirection(Vector2.left);
        CheckAvailableDirection(Vector2.right);
    }

    private void CheckAvailableDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1.0f, obstacleLayer);

        // If no collider is hit then there is no obstacle in that direction
        if (hit.collider == null)
        {
            availableDirections.Add(direction);
        }
    }

}
