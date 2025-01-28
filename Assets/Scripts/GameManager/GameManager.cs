using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public PlayerManager playerManager;
    public LeaderboardManager leaderboardManager;
    public DifficultyManager difficultyManager;
    public UIManager uiManager;
    public AudioManager audioManager;

    private string playerName;
    private int score = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        uiManager.InitializeUI();
        leaderboardManager.LoadLeaderboard();
        leaderboardManager.DisplayLeaderboard();
    }

    public void StartGame()
    {
        playerName = uiManager.GetPlayerName();
        score = 0;
        playerManager.ResetGame();
        uiManager.UpdateScoreUI(score);
        audioManager.PlayStartSound();
    }

    public void EndGame()
    {
        leaderboardManager.AddToLeaderboard(playerName, score, difficultyManager.CurrentDifficulty);
        leaderboardManager.SaveLeaderboard();
        uiManager.ShowGameOver(score);
        audioManager.PlayGameOverSound();
    }

    public void CheckShapeMatch(string incomingShape)
    {
        bool isMatch = playerManager.CheckShapeMatch(incomingShape);
        if (isMatch)
        {
            score++;
            uiManager.UpdateScoreUI(score);
            audioManager.PlayScoreSound();
        }
        else
        {
            EndGame();
        }
    }
}
