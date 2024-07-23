using UnityEngine;

public class Fruit : MonoBehaviour
{
    [Tooltip("Layer mask for detecting Pacman.")]
    [SerializeField] private LayerMask pacmanLayerMask;

    [Tooltip("Type of fruit.")]
    [SerializeField] private FruitType fruitType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((pacmanLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            EatFruit();
        }
    }

    private void EatFruit()
    {
        GameManager.Instance.SetScore(GameManager.Instance.Score + (int)fruitType);

        Destroy(gameObject);
    }
}
