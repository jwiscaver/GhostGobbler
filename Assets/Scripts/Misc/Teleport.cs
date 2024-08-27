using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Teleport : MonoBehaviour
{
    [SerializeField] public Transform connection;

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.transform.position = connection.position;
    }
}