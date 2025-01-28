using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Cinemachine;
using System;
using System.Linq;
using System.IO;

public enum Difficulty
{
    Easy,
    Medium,
    Hard,
    UltraMode
}

[System.Serializable]
public class LeaderboardEntry
{
    public string PlayerName;
    public int Score;
    public Difficulty Difficulty;

    public LeaderboardEntry(string playerName, int score, Difficulty difficulty)
    {
        PlayerName = playerName;
        Score = score;
        Difficulty = difficulty;
    }
}

[System.Serializable]
public class SerializableLeaderboard
{
    public List<LeaderboardEntry> Entries = new List<LeaderboardEntry>();

    public SerializableLeaderboard(List<LeaderboardEntry> leaderboard)
    {
        Entries = leaderboard;
    }

    public List<LeaderboardEntry> ToList()
    {
        return Entries;
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public PlayerShapes playerShapePrefab;
    public ShapeSpawner spawnerPrefab;
    public Transform spawnArea;
    public Transform cameraTarget;
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineTransposer transposer;
    private int score = 0;
    private int spawnCount = 0;

    [Header("UI Settings")]
    public TextMeshProUGUI scoreText;
    public GameObject gameOverCanvas;
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI highScoreTextMenu;
    public TMP_InputField playerNameInputField;
    public GameObject leaderboardRowPrefab;
    public Transform leaderboardContent;

    [Header("Audio Settings")]
    public AudioClip scoreSound;
    public AudioClip spawnSound;
    public AudioClip gameoverSound;
    private AudioSource audioSource;

    [Header("Difficulty Settings")]
    private Difficulty currentDifficulty;
    public float minSpawnInterval = 8f;
    public float maxSpawnInterval = 10f;
    public float moveSpeed = 4f;

    private List<int> spawnThresholds = new List<int> { 6, 16, 31, 51 }; // default value { 6, 16, 31, 51 }
    public List<PlayerShapes> players = new List<PlayerShapes>();
    private HashSet<int> triggeredThresholds = new HashSet<int>();
    private List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();
    private string playerName;
    private const string LeaderboardKey = "LeaderboardData";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
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
        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        LoadLeaderboard();
        DisplayLeaderboard();

        int highScore = leaderboard.Count > 0 ? leaderboard.Max(entry => entry.Score) : 0;
        highScoreTextMenu.text = "High Score: " + highScore;
    }

    public void StartGame()
    {
        playerName = string.IsNullOrWhiteSpace(playerNameInputField.text) ? "Player" : playerNameInputField.text;

        score = 0;
        spawnCount = 0;
        triggeredThresholds.Clear();
        UpdateScoreUI();
        SpawnNewSpawnerAndPlayer(isFirstSpawn: true);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void CheckShapeMatch(string incomingShape)
    {
        PlayerShapes currentPlayer = players.FirstOrDefault(player => player.GetCurrentShapeName() == incomingShape);

        if (currentPlayer != null)
        {
            PlaySound(scoreSound);
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
        AddToLeaderboard(playerName, score, currentDifficulty);

        int highScore = leaderboard.Count > 0 ? leaderboard.Max(entry => entry.Score) : 0;

        currentScoreText.text = "Current Score: " + score;
        highScoreText.text = "High Score: " + highScore;

        foreach (var player in players)
        {
            player.isGameOver = true;
        }

        Time.timeScale = 0f;
        PlaySound(gameoverSound);
        gameOverCanvas.SetActive(true);
        DisplayLeaderboard();
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
            spawnerPosition = new Vector3(0, 0, 25);
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

            spawnerPosition = new Vector3(0 + xOffset, 0, 25);
            playerPosition = new Vector3(0 + xOffset, 0, -5);
        }

        ShapeSpawner newSpawner = Instantiate(spawnerPrefab, spawnerPosition, Quaternion.identity, spawnArea);
        PlayerShapes newPlayer = Instantiate(playerShapePrefab, playerPosition, Quaternion.identity, spawnArea);

        newSpawner.player = newPlayer.transform;
        players.Add(newPlayer);
        spawnCount++;

        PlaySound(spawnSound);
        UpdateCameraTargetPosition();
    }

    private void UpdateCameraTargetPosition()
    {
        if (players.Count == 0) return;

        Vector3 center = players.Aggregate(Vector3.zero, (acc, player) => acc + player.transform.position) / players.Count;

        cameraTarget.position = new Vector3(center.x, 0, -players.Count + 1.5f);

        float targetYPosition = players.Count + 5f;
        float smoothSpeed = 5f;
        transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, new Vector3(transposer.m_FollowOffset.x, targetYPosition, transposer.m_FollowOffset.z), Time.deltaTime * smoothSpeed);
    }

    private void AddToLeaderboard(string playerName, int score, Difficulty difficulty)
    {
        leaderboard.Add(new LeaderboardEntry(playerName, score, difficulty));
        SaveLeaderboard();
    }

    private void DisplayLeaderboard()
    {
        foreach (Transform child in leaderboardContent)
        {
            Destroy(child.gameObject);
        }

        var sortedEntries = leaderboard.OrderByDescending(entry => entry.Score);

        foreach (var entry in sortedEntries)
        {
            GameObject newRow = Instantiate(leaderboardRowPrefab, leaderboardContent);

            TextMeshProUGUI[] texts = newRow.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = entry.PlayerName;
            texts[1].text = entry.Difficulty.ToString();
            texts[2].text = entry.Score.ToString();
        }
    }

    public void SortLeaderboardByScore()
    {
        leaderboard = leaderboard.OrderByDescending(entry => entry.Score).ToList();
        DisplayLeaderboard();
    }

    public void SortLeaderboardByDifficulty()
    {
        leaderboard = leaderboard.OrderBy(entry => entry.Difficulty).ToList();
        DisplayLeaderboard();
    }

    private void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(new SerializableLeaderboard(leaderboard));
        PlayerPrefs.SetString(LeaderboardKey, json);
        PlayerPrefs.Save();
    }

    private void LoadLeaderboard()
    {
        if (PlayerPrefs.HasKey(LeaderboardKey))
        {
            string json = PlayerPrefs.GetString(LeaderboardKey);
            SerializableLeaderboard savedLeaderboard = JsonUtility.FromJson<SerializableLeaderboard>(json);
            leaderboard = savedLeaderboard.ToList();
        }
    }

    public void ClearLeaderboard()
    {
        PlayerPrefs.DeleteKey(LeaderboardKey);
        leaderboard.Clear();
        DisplayLeaderboard();
    }
}
