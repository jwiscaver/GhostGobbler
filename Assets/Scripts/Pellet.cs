using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Pellet : MonoBehaviour
{
    [Tooltip("Points obtained by eating pellet.")]
    [SerializeField] private int points = 10;

    [Tooltip("Layer mask for detecting Pacman.")]
    [SerializeField] private LayerMask pacmanLayer;

    public int Points => points;

    protected virtual void Eat()
    {
        GameManager.Instance.PelletEaten(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & pacmanLayer) != 0)
        {
            Eat();
        }
    }
}
