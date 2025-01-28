using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject startUI;
    public GameObject gameUI;
    public GameObject gameOverCanvas;
    public GameManager gameManager;
    public DifficultyManager difficultyManager;

    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;
    public Button ultraModeButton;
    public TMP_InputField playerNameInputField;
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        InitializeUI();

        easyButton.onClick.AddListener(() => SetDifficulty(Difficulty.Easy));
        mediumButton.onClick.AddListener(() => SetDifficulty(Difficulty.Medium));
        hardButton.onClick.AddListener(() => SetDifficulty(Difficulty.Hard));
        ultraModeButton.onClick.AddListener(() => SetDifficulty(Difficulty.UltraMode));
    }

    public void InitializeUI()
    {
        startUI.SetActive(true);
        gameUI.SetActive(false);
        gameOverCanvas.SetActive(false);
    }

    public void SetDifficulty(Difficulty difficulty)
    {
        difficultyManager.SetDifficulty(difficulty);
        startUI.SetActive(false);
        gameUI.SetActive(true);
        gameManager.StartGame();
    }

    public void UpdateScoreUI(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public string GetPlayerName()
    {
        return string.IsNullOrWhiteSpace(playerNameInputField.text) ? "Player" : playerNameInputField.text;
    }

    public void ShowGameOver(int score)
    {
        gameOverCanvas.SetActive(true);
        // Update Game Over UI elements
    }
}
