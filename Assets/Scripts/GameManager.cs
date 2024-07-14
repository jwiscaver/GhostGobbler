using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Tooltip("Array of ghosts in the game.")]
    [SerializeField] private Ghost[] ghosts;

    [Tooltip("Pacman player character.")]
    [SerializeField] private Pacman pacman;

    [Tooltip("Parent transform containing all pellet objects.")]
    [SerializeField] private Transform pellets;

    [Tooltip("TextMeshProUGUI component for displaying 'Game Over' text.")]
    [SerializeField] private TextMeshProUGUI gameOverText;

    [Tooltip("TextMeshProUGUI component for displaying the score.")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Tooltip("Transform for displaying lives on the UI canvas.")]
    [SerializeField] private Transform livesDisplay;

    [Tooltip("Sprite for displaying lives.")]
    [SerializeField] private Sprite lifeSprite;

    [Tooltip("TextMeshProUGUI component for displaying 'Ready' text.")]
    [SerializeField] private TextMeshProUGUI readyText;

    private int ghostMultiplier = 1;
    private int lives = 3;
    private int score = 0;
    private bool isGameReady = false;

    private List<Image> lifeImages = new List<Image>();

    public int Lives => lives;
    public int Score => score;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        // Cache references to the life images
        foreach (Transform child in livesDisplay)
        {
            Image image = child.GetComponent<Image>();
            if (image != null)
            {
                lifeImages.Add(image);
            }
        }
        StartNewGame();
    }

    private void Update()
    {
        if (lives <= 0 && Input.anyKeyDown)
        {
            StartNewGame();
        }
    }

    private void StartNewGame()
    {
        SetScore(0);
        SetLives(3);
        StartNewRound();
    }

    private void StartNewRound()
    {
        gameOverText.enabled = false;
        ShowReadyText();

        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }

        ResetGameState();
    }

    private void ResetGameState()
    {
        foreach (Ghost ghost in ghosts)
        {
            ghost.ResetState();
            ghost.GetComponent<Movement>().enabled = false; // Disable ghost movement until game is ready
        }

        pacman.ResetState();
        pacman.GetComponent<Movement>().enabled = false; // Disable Pacman movement until game is ready
    }

    private void EndGame()
    {
        gameOverText.enabled = true;

        foreach (Ghost ghost in ghosts)
        {
            ghost.gameObject.SetActive(false);
        }

        pacman.gameObject.SetActive(false);
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        UpdateLivesDisplay();
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString("D2");
    }

    private void UpdateLivesDisplay()
    {
        for (int i = 0; i < lifeImages.Count; i++)
        {
            lifeImages[i].enabled = i < lives;
        }
    }

    private void ShowReadyText()
    {
        readyText.enabled = true;
        StartCoroutine(HideReadyTextAfterDelay(3f));  // Display for 3 seconds
    }

    private IEnumerator HideReadyTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        readyText.enabled = false;
        StartGame();
    }

    private void StartGame()
    {
        isGameReady = true;

        foreach (Ghost ghost in ghosts)
        {
            ghost.GetComponent<Movement>().enabled = true; // Enable ghost movement
        }

        pacman.GetComponent<Movement>().enabled = true; // Enable Pacman movement
    }

    public void PacmanEaten()
    {
        pacman.DeathSequence();

        SetLives(lives - 1);

        if (lives > 0)
        {
            StartCoroutine(ResetStateAfterDelay(3f));
        }
        else
        {
            EndGame();
        }
    }

    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier;
        SetScore(score + points);

        ghostMultiplier++;
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);

        SetScore(score + pellet.Points);

        if (!HasRemainingPellets())
        {
            pacman.gameObject.SetActive(false);
            StartCoroutine(StartNewRoundAfterDelay(3f));
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        foreach (Ghost ghost in ghosts)
        {
            ghost.frightened.Enable(pellet.duration);
        }

        PelletEaten(pellet);
        StopCoroutine(nameof(ResetGhostMultiplierAfterDelay));
        StartCoroutine(ResetGhostMultiplierAfterDelay(pellet.duration));
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator ResetStateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetGameState();
        ShowReadyText(); // Show "Ready" text before restarting the game
    }

    private IEnumerator StartNewRoundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartNewRound();
    }

    private IEnumerator ResetGhostMultiplierAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ghostMultiplier = 1;
    }
}
