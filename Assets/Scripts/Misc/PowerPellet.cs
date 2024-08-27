using UnityEngine;

[RequireComponent(typeof(FlashBehavior))]
public class PowerPellet : Pellet
{
    public float duration = 9f;
    private FlashBehavior flashBehavior;

    private void Start()
    {
        flashBehavior = GetComponent<FlashBehavior>();

        if (flashBehavior != null)
        {
            flashBehavior.Flash();
        }
        else
        {
            Debug.LogError("FlashBehavior is missing on the PowerPellet GameObject.");
        }
    }

    private void OnEnable()
    {
        if (flashBehavior != null)
        {
            flashBehavior.Flash();
        }
    }

    protected override void Eat()
    {
        GameManager.Instance.PowerPelletEaten(this);
    }
}