using UnityEngine;

public class PowerPellet : Pellet
{
    [Tooltip("How long the pellet lasts before it disappears.")]
    [SerializeField] public float duration = 8f;

    protected override void Eat()
    {
        GameManager.Instance.PowerPelletEaten(this);
    }
}
