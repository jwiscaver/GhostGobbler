using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        AudioManager.Instance.PauseAllAudio();
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        AudioManager.Instance.ResumeAllAudio();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;     
        isPaused = false;   
        AudioManager.Instance.StopAllAudio();
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene("Menu");
    }
}