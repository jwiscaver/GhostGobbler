using UnityEngine;

[RequireComponent(typeof(FlashBehavior))]
public class PowerPellet : Pellet
{
    public float duration = 9f;
    private FlashBehavior flashBehavior;

    void Awake()
    {
        flashBehavior = GetComponent<FlashBehavior>();
    }

    private void Start()
    {
        flashBehavior.Flash();
    }

    protected override void Eat()
    {
        GameManager.Instance.PowerPelletEaten(this);
    }
}