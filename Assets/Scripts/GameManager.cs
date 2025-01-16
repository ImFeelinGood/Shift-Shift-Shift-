using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public enum Difficulty
{
    Easy,
    Medium,
    Hard,
    UltraMode
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerShapes playerShapePrefab;
    public ShapeSpawner spawnerPrefab;
    public Transform spawnArea;

    public TextMeshProUGUI scoreText;
    public GameObject gameOverCanvas;
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;

    private int score = 0;
    private int spawnCount = 0;
    private Difficulty currentDifficulty;

    public float minSpawnInterval = 8f;
    public float maxSpawnInterval = 10f;
    public float moveSpeed = 4f;

    private List<int> spawnThresholds = new List<int> { 6, 16, 21, 51 };
    public List<PlayerShapes> players = new List<PlayerShapes>();
    private HashSet<int> triggeredThresholds = new HashSet<int>();
    private Vector3 lastSpawnerPosition = Vector3.zero;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetDifficulty(Difficulty difficulty)
    {
        currentDifficulty = difficulty;

        switch (difficulty)
        {
            case Difficulty.Easy:
                minSpawnInterval = 8f;
                maxSpawnInterval = 10f;
                moveSpeed = 4f;
                break;
            case Difficulty.Medium:
                minSpawnInterval = 6f;
                maxSpawnInterval = 10f;
                moveSpeed = 5f;
                break;
            case Difficulty.Hard:
                minSpawnInterval = 4f;
                maxSpawnInterval = 8f;
                moveSpeed = 6f;
                break;
            case Difficulty.UltraMode:
                minSpawnInterval = 3f;
                maxSpawnInterval = 6f;
                moveSpeed = 7f;
                break;
        }

        UpdateSpawnerProperties();
    }

    private void UpdateSpawnerProperties()
    {
        ShapeSpawner spawner = spawnerPrefab.GetComponent<ShapeSpawner>();
        if (spawner != null)
        {
            spawner.minSpawnInterval = minSpawnInterval;
            spawner.maxSpawnInterval = maxSpawnInterval;
            spawner.moveSpeed = moveSpeed;
        }
    }

    private void Start()
    {
        gameOverCanvas.SetActive(false);
    }

    public void StartGame()
    {
        score = 0;
        spawnCount = 0;
        triggeredThresholds.Clear();
        UpdateScoreUI();
        SpawnNewSpawnerAndPlayer(isFirstSpawn: true);
    }

    public void CheckShapeMatch(string incomingShape)
    {
        PlayerShapes currentPlayer = null;

        foreach (var player in players)
        {
            if (player.GetCurrentShapeName() == incomingShape)
            {
                currentPlayer = player;
                break;
            }
        }

        if (currentPlayer != null)
        {
            score++;
            CheckAndSpawnNewElements();
        }
        else
        {
            EndGame();
        }

        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }

    public void EndGame()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        currentScoreText.text = "Current Score: " + score;
        highScoreText.text = "High Score: " + highScore;

        Time.timeScale = 0f;
        gameOverCanvas.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void CheckAndSpawnNewElements()
    {
        if (spawnThresholds.Contains(score) && !triggeredThresholds.Contains(score))
        {
            triggeredThresholds.Add(score);
            SpawnNewSpawnerAndPlayer();
        }
    }

    private void SpawnNewSpawnerAndPlayer(bool isFirstSpawn = false)
    {
        Vector3 spawnerPosition;
        Vector3 playerPosition;

        if (isFirstSpawn)
        {
            spawnerPosition = new Vector3(0, 0, 15);
            playerPosition = new Vector3(0, 0, -5);
        }
        else
        {
            float xOffset = 0;
            switch (spawnCount)
            {
                case 1: xOffset = 2.5f; break;
                case 2: xOffset = -2.5f; break;
                case 3: xOffset = 5f; break;
                case 4: xOffset = -5f; break;
                default: xOffset = 0; break;
            }

            spawnerPosition = new Vector3(0 + xOffset, 0, 15);
            playerPosition = new Vector3(0 + xOffset, 0, -5);
        }

        ShapeSpawner newSpawner = Instantiate(spawnerPrefab, spawnerPosition, Quaternion.identity, spawnArea);
        PlayerShapes newPlayer = Instantiate(playerShapePrefab, playerPosition, Quaternion.identity, spawnArea);

        newSpawner.player = newPlayer.transform;
        players.Add(newPlayer);
        spawnCount++;
    }
}
