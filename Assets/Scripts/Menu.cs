using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class Menu : MonoBehaviour
{
    public GameObject infoMenuUI;
    public TextMeshProUGUI hoverText;

    void Start()
    {
        hoverText.text = "";
        infoMenuUI.SetActive(false);
    }

    public void OnHoverEnter(string text)
    {
        hoverText.text = text;
    }

    public void OnHoverExit()
    {
        hoverText.text = "";
    }

    public void ClassicMode()
    {
        SceneManager.LoadScene("GhostGobbler");
    }

    public void ExtraMode()
    {
        SceneManager.LoadScene("GhostGobbler");
    }

    public void Information()
    {
        infoMenuUI.SetActive(!infoMenuUI.activeSelf);
    }
}
