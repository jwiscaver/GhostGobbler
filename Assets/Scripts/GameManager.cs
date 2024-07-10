using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

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

    [Tooltip("TextMeshProUGUI component for displaying the lives remaining.")]
    [SerializeField] private TextMeshProUGUI livesText;

    private int ghostMultiplier = 1;
    private int lives = 3;
    private int score = 0;

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
        NewGame();
    }

    private void Update()
    {
        if (lives <= 0 && Input.anyKeyDown)
        {
            NewGame();
        }
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        gameOverText.enabled = false;

        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }

        ResetPacmanAndGhosts();
    }

    private void ResetPacmanAndGhosts()
    {
        foreach (Ghost ghost in ghosts)
        {
            ghost.ResetState();
        }

        pacman.ResetState();
    }

    private void GameOver()
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
        livesText.text = $"x{lives}";
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString("D2");
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
            GameOver();
        }
    }

    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.Points * ghostMultiplier;
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
            StartCoroutine(NewRoundAfterDelay(3f));
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        /*foreach (Ghost ghost in ghosts)
        {
            ghost.frightened.Enable(pellet.duration);
        }*/

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
        ResetPacmanAndGhosts();
    }

    private IEnumerator NewRoundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        NewRound();
    }

    private IEnumerator ResetGhostMultiplierAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ghostMultiplier = 1;
    }
}