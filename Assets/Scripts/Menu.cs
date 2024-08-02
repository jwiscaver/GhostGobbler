using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class Menu : MonoBehaviour
{
    public TextMeshProUGUI hoverText;

    void Start()
    {
        hoverText.text = "";
    }

    // Method to show text
    public void OnHoverEnter(string text)
    {
        hoverText.text = text;
    }

    // Method to hide text
    public void OnHoverExit()
    {
        hoverText.text = "";
    }

    public void ClassicMode()
    {
        SceneManager.LoadScene("GhostGobbler");
    }
}
