using UnityEngine;

public class PowerPellet : Pellet
{
    public float duration = 9f;

    protected override void Eat()
    {
        GameManager.Instance.PowerPelletEaten(this);
    }

}
