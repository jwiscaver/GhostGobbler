using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Pellet : MonoBehaviour
{
    private int points = 10;

    public int Points => points;

    protected virtual void Eat()
    {
        GameManager.Instance.PelletEaten(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            Eat();
        }
    }
}