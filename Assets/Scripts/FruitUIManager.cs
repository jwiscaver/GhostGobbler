using UnityEngine;
using UnityEngine.UI;

public class FruitUIManager : MonoBehaviour
{
    [SerializeField] private Image[] fruitImages; // Array of UI Images to display fruits
    [SerializeField] private Sprite[] fruitSprites; // Array of fruit sprites

    private void Start()
    {
        UpdateFruitUI(0); // Initialize with level 0
    }

    public void UpdateFruitUI(int level)
    {
        if (level < 8)
        {
            for (int i = 0; i < fruitImages.Length; i++)
            {
                if (i <= level)
                {
                    fruitImages[i].sprite = fruitSprites[i];
                    fruitImages[i].enabled = true;
                }
                else
                {
                    fruitImages[i].enabled = false;
                }
            }
        }
        else if (level < 19)
        {
            int offset = level - 8;
            for (int i = 0; i < fruitImages.Length; i++)
            {
                if (i + offset < fruitSprites.Length)
                {
                    fruitImages[i].sprite = fruitSprites[i + offset];
                    fruitImages[i].enabled = true;
                }
                else
                {
                    fruitImages[i].enabled = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < fruitImages.Length; i++)
            {
                fruitImages[i].sprite = fruitSprites[fruitSprites.Length - 1]; // Key sprite
                fruitImages[i].enabled = true;
            }
        }
    }
}
