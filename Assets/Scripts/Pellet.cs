using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Pellet : MonoBehaviour
{
    [Tooltip("Points awarded for eating this pellet.")]
    [SerializeField] private int points = 10;

    [Tooltip("Layer mask for detecting Pacman.")]
    [SerializeField] private LayerMask pacmanLayerMask;

    public int Points => points;

    protected virtual void Eat()
    {
        GameManager.Instance.PelletEaten(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((pacmanLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            Eat();
        }
    }
}
