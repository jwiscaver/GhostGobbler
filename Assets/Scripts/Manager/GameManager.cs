using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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

    [Tooltip("TextMeshProUGUI component for displaying the high score.")]
    [SerializeField] private TextMeshProUGUI highScoreText;

    [Tooltip("Transform for displaying lives on the UI canvas.")]
    [SerializeField] private Transform livesDisplay;

    [Tooltip("Sprite for displaying lives.")]
    [SerializeField] private Sprite lifeSprite;

    [Tooltip("TextMeshProUGUI component for displaying 'Ready' text.")]
    [SerializeField] private TextMeshProUGUI readyText;

    [Tooltip("Initial number of lives.")]
    [SerializeField] private int initialLives = 3;

    [Tooltip("Delay before hiding the 'Ready' text (in seconds).")]
    [SerializeField] private float readyTextDisplayTime = 4f;

    [Tooltip("Delay before resetting the game state after Pacman is eaten (in seconds).")]
    [SerializeField] private float resetStateDelay = 3f;

    [Tooltip("Delay before starting a new round after all pellets are eaten (in seconds).")]
    [SerializeField] private float newRoundDelay = 3f;

    [Tooltip("Delay before resetting the ghost multiplier after a power pellet is eaten (in seconds).")]
    [SerializeField] private float resetGhostMultiplierDelay = 3f;

    [Tooltip("Fruit UI Manager")]
    [SerializeField] private FruitUIManager fruitUIManager;

    [Tooltip("Fruit Prefabs")]
    [SerializeField] private GameObject[] fruitPrefabs;

    [Tooltip("Fruit Spawn Point")]
    [SerializeField] private Transform fruitSpawnPoint;

    [Tooltip("Prefab for displaying points when a ghost is eaten.")]
    [SerializeField] private GameObject pointsDisplayPrefab;

    private int ghostMultiplier = 1;
    private int lives;
    private int score = 0;
    private int highScore = 0;
    private int currentLevel = 0;
    private bool isGameReady = false;
    private bool fruitActive = false;
    private int pelletsEaten = 0;
    private GameObject currentFruit;
    private Coroutine fruitCoroutine;
    private Canvas gameUICanvas;

    private List<Image> lifeImages = new List<Image>();
    private List<GhostMovement> ghostMovements = new List<GhostMovement>();
    private Movement pacmanMovement;

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
        foreach (Transform child in livesDisplay)
        {
            Image image = child.GetComponent<Image>();
            if (image != null)
            {
                lifeImages.Add(image);
            }
        }

        foreach (Ghost ghost in ghosts)
        {
            GhostMovement movement = ghost.GetComponent<GhostMovement>();
            if (movement != null)
            {
                ghostMovements.Add(movement);
            }
        }

        pacmanMovement = pacman.GetComponent<Movement>();

        LoadHighScore();
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
        SetLives(initialLives);
        currentLevel = 0;
        StartNewRound();
    }

    private void StartNewRound()
    {
        gameOverText.enabled = false;
        AudioManager.Instance.PlayIntro();
        ShowReadyText();

        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }

        pelletsEaten = 0; // Reset the pellets eaten counter
        fruitActive = false; // Reset fruit active state
        ResetGameState();
        fruitUIManager.UpdateFruitUI(currentLevel);
    }

    private void ResetGameState()
    {
        foreach (GhostMovement ghostMovement in ghostMovements)
        {
            ghostMovement.GetComponent<Ghost>().ResetState();
            ghostMovement.enabled = false;
        }

        pacman.ResetState();
        pacmanMovement.enabled = false;
    }

    private void EndGame()
    {
        gameOverText.enabled = true;

        foreach (Ghost ghost in ghosts)
        {
            ghost.gameObject.SetActive(false);
        }

        pacman.gameObject.SetActive(false);

        if (score > highScore)
        {
            SetHighScore(score);
        }
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        UpdateLivesDisplay();
    }

    public void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString("D2");

        if (score > highScore)
        {
            SetHighScore(score);
        }
    }

    private void SetHighScore(int score)
    {
        highScore = score;
        highScoreText.text = highScore.ToString("D2");
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
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
        StartCoroutine(HideReadyTextAfterDelay(readyTextDisplayTime));
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

        foreach (GhostMovement ghostMovement in ghostMovements)
        {
            ghostMovement.enabled = true;
        }

        pacmanMovement.enabled = true;

        AudioManager.Instance.PlayNormalGhostMusic();
    }

    public void PacmanEaten()
    {
        StartCoroutine(PacmanDeathSequence());
    }

    private IEnumerator PacmanDeathSequence()
    {
        AudioManager.Instance.StopChomp();
        AudioManager.Instance.StopNormalGhostMusic();
        AudioManager.Instance.PlayDeath();

        pacmanMovement.enabled = false;
        foreach (GhostMovement ghostMovement in ghostMovements)
        {
            ghostMovement.enabled = false;
        }

        // Short delay before playing the death animation
        yield return new WaitForSeconds(1f);

        // Hide the ghosts
        foreach (Ghost ghost in ghosts)
        {
            ghost.gameObject.SetActive(false);
        }

        // Play death animation
        pacman.DeathSequence();

        // Wait for the death animation to finish (assuming 1 second for the animation)
        yield return new WaitForSeconds(1f);

        SetLives(lives - 1);

        if (lives > 0)
        {
            StartCoroutine(ResetStateAfterDelay(resetStateDelay));
        }
        else
        {
            EndGame();
        }
    }

    public void GhostEaten(Ghost ghost)
    {
        // Points calculation based on ghostMultiplier
        int points = 200 * (int)Mathf.Pow(2, ghostMultiplier - 1);
        SetScore(score + points);

        AudioManager.Instance.PlayGhostKill();

        ShowPoints(ghost, points);

        // Increase the ghost multiplier for the next ghost
        ghostMultiplier++;

        StartCoroutine(GhostEatenDelay(1f)); // Start the coroutine
    }

    private IEnumerator GhostEatenDelay(float delay)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(delay); // Use WaitForSecondsRealtime for real-time delay
        Time.timeScale = 1f;
    }

    private void ShowPoints(Ghost ghost, int points)
    {
        if (pointsDisplayPrefab != null)
        {
            GameObject popupInstance = Instantiate(pointsDisplayPrefab);
            gameUICanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();

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

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);
        pelletsEaten++;

        Debug.Log($"Pellet eaten: {pelletsEaten}");

        CheckFruitSpawn();

        SetScore(score + pellet.Points);

        if (!HasRemainingPellets())
        {
            pacman.gameObject.SetActive(false);
            StartCoroutine(StartNewRoundAfterDelay(newRoundDelay));
        }
    }

    private IEnumerator PlayMusicTransitionFromFrightenedToNormal(float duration)
    {
        AudioManager.Instance.PlayGhostFrightened();
        yield return new WaitForSeconds(duration);
        AudioManager.Instance.StopGhostFrightened();
        AudioManager.Instance.PlayNormalGhostMusic();
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        foreach (Ghost ghost in ghosts)
        {
            ghost.frightened.Enable(pellet.duration);
        }

        PelletEaten(pellet);
        StartCoroutine(PlayMusicTransitionFromFrightenedToNormal(pellet.duration));
        StopCoroutine(nameof(ResetGhostMultiplierAfterDelay));
        StartCoroutine(ResetGhostMultiplierAfterDelay(resetGhostMultiplierDelay));
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
        ShowReadyText();
        AudioManager.Instance.PlayIntro();
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

    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = highScore.ToString("D2");
    }

    private void CheckFruitSpawn()
    {
        Debug.Log($"Pellets Eaten: {pelletsEaten}, Fruit Active: {fruitActive}");
        if ((pelletsEaten == 70 || pelletsEaten == 170) && !fruitActive)
        {
            FruitType fruitType = GetFruitTypeForLevel(currentLevel);
            Debug.Log($"Attempting to spawn fruit: {fruitType} for level {currentLevel}");
            SpawnFruit(fruitType);
        }
    }

    private void SpawnFruit(FruitType fruitType)
    {
        if (currentFruit != null)
        {
            Destroy(currentFruit);
        }

        Debug.Log($"Spawning fruit: {fruitType}");

        try
        {
            int index = GetFruitPrefabIndex(fruitType);
            Debug.Log($"Spawning fruit: {fruitType} at index {index}");
            currentFruit = Instantiate(fruitPrefabs[index], fruitSpawnPoint.position, Quaternion.identity);
            fruitActive = true;
            fruitCoroutine = StartCoroutine(FruitLifetimeCoroutine());
        }
        catch (System.ArgumentOutOfRangeException ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    private int GetFruitPrefabIndex(FruitType fruitType)
    {
        switch (fruitType)
        {
            case FruitType.Cherry:
                return 0;
            case FruitType.Strawberry:
                return 1;
            case FruitType.Orange:
                return 2;
            case FruitType.Apple:
                return 3;
            case FruitType.Melon:
                return 4;
            case FruitType.Galaxian:
                return 5;
            case FruitType.Bell:
                return 6;
            case FruitType.Key:
                return 7;
            default:
                throw new System.ArgumentOutOfRangeException($"FruitType {fruitType} not mapped to a prefab index.");
        }
    }

    private FruitType GetFruitTypeForLevel(int level)
    {
        // Define the order of fruits
        FruitType[] fruitOrder = new FruitType[]
        {
            FruitType.Cherry,
            FruitType.Strawberry,
            FruitType.Orange,
            FruitType.Apple,
            FruitType.Melon,
            FruitType.Galaxian,
            FruitType.Bell,
            FruitType.Key
        };

        if (level < 8)
        {
            return fruitOrder[level];
        }
        else
        {
            return fruitOrder[(level - 8) % 7]; // Rotate between the first 7 fruits after level 7
        }
    }

    private IEnumerator FruitLifetimeCoroutine()
    {
        yield return new WaitForSeconds(9f); // Set the fruit lifetime duration
        if (currentFruit != null)
        {
            Destroy(currentFruit);
            fruitActive = false;
        }
    }

    public void FruitEaten()
    {
        fruitActive = false;
    }

    private void LoadNextLevel()
    {
        currentLevel++;
        Debug.Log($"Loading Next Level: {currentLevel}");
        StartNewRound();
    }
}
