using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Tooltip("TextMeshProUGUI component for displaying points from eating fruit.")]
    [SerializeField] private TextMeshProUGUI fruitPointsText;

    [SerializeField] private float displayDuration = 2.0f;

    private void Start()
    {
        fruitPointsText.gameObject.SetActive(false);
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
}
