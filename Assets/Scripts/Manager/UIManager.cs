using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [Tooltip("TextMeshProUGUI component for displaying points from eating fruit.")]
    [SerializeField] private TextMeshProUGUI fruitPointsText;

    [SerializeField] private float displayDuration = 1.0f;

    [Tooltip("TextMeshProUGUI component for displaying the score.")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Tooltip("TextMeshProUGUI component for displaying the high score.")]
    [SerializeField] private TextMeshProUGUI highScoreText;

    [Tooltip("Prefab for displaying points when a ghost is eaten.")]
    [SerializeField] private GameObject pointsDisplayPrefab;

    [Tooltip("Transform for displaying lives on the UI canvas.")]
    [SerializeField] private Transform livesDisplay;

    private Canvas gameUICanvas;
    private List<Image> lifeImages = new List<Image>();

    private void Start()
    {
        fruitPointsText.gameObject.SetActive(false);
        gameUICanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();

        foreach (Transform child in livesDisplay)
        {
            Image image = child.GetComponent<Image>();
            if (image != null)
            {
                lifeImages.Add(image);
            }
        }
    }

    public void ShowPoints(Ghost ghost, int points)
    {
        if (pointsDisplayPrefab != null)
        {
            GameObject popupInstance = Instantiate(pointsDisplayPrefab);

            RectTransform rectTransform = popupInstance.GetComponent<RectTransform>();
            rectTransform.SetParent(gameUICanvas.transform, false);

            Vector2 screenPosition = Camera.main.WorldToScreenPoint(ghost.transform.position);
            rectTransform.position = screenPosition;

            TMP_Text pointsText = popupInstance.GetComponent<TMP_Text>();
            if (pointsText != null)
            {
                pointsText.text = "+" + points.ToString();
            }

            Destroy(popupInstance, 1.0f);
        }
    }

    public void ShowPoints(int points)
    {
        fruitPointsText.text = "+" + points.ToString();
        StartCoroutine(DisplayPointsCoroutine());
    }

    private IEnumerator DisplayPointsCoroutine()
    {
        fruitPointsText.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        fruitPointsText.gameObject.SetActive(false);
    }

    public void UpdateScoreUI(int score)
    {
        scoreText.text = score.ToString("D2");
    }

    public void UpdateHighScoreUI(int highScore)
    {
        highScoreText.text = highScore.ToString("D2");
    }

    public void UpdateLivesDisplay(int lives)
    {
        for (int i = 0; i < lifeImages.Count; i++)
        {
            lifeImages[i].enabled = i < lives;
        }
    }
}