using UnityEngine;
using UnityEngine.UI;

public class FruitUIManager : MonoBehaviour
{
    [SerializeField] private Image[] fruitImages;
    [SerializeField] private Sprite[] fruitSprites;

    private int[] fruitIndices = { 0, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7 }; // Indices for each level

    private void Start()
    {
        UpdateFruitUI(0);
    }

    public void UpdateFruitUI(int level)
    {
        // Determine the fruit to show based on the level
        int fruitIndex = GetFruitIndexForLevel(level);

        // Update the UI to show collected fruits
        for (int i = 0; i < fruitImages.Length; i++)
        {
            if (i < level)
            {
                fruitImages[i].sprite = fruitSprites[GetFruitIndexForLevel(i)];
                fruitImages[i].enabled = true;
            }
            else if (i == level)
            {
                fruitImages[i].sprite = fruitSprites[fruitIndex];
                fruitImages[i].enabled = true;
            }
            else
            {
                fruitImages[i].enabled = false;
            }
        }
    }

    private int GetFruitIndexForLevel(int level)
    {
        if (level < 13)
        {
            return fruitIndices[level];
        }
        else
        {
            return 7;
        }
    }
}
