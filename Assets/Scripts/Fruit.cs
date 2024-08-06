using UnityEngine;

public class Fruit : MonoBehaviour
{
    [Tooltip("Layer mask for detecting Pacman.")]
    [SerializeField] private LayerMask pacmanLayerMask;

    [Tooltip("Type of fruit.")]
    [SerializeField] private FruitType fruitType;

    private UIManager uiManager;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((pacmanLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            EatFruit();
        }
    }

    private void EatFruit()
    {
        int points = (int)fruitType;
        GameManager.Instance.SetScore(GameManager.Instance.Score + points);

        if (uiManager != null)
        {
            gameObject.SetActive(false);
            uiManager.ShowPoints(points);
        }

        AudioManager.Instance.PlayFruitCollect();
        GameManager.Instance.FruitEaten(); // Notify GameManager
        Destroy(gameObject);
    }
}
