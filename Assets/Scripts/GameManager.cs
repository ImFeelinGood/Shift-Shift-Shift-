using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerShapes playerShape;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverCanvas;
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;

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
        UpdateScoreUI();
        gameOverCanvas.SetActive(false);
    }

    public void CheckShapeMatch(string incomingShape)
    {
        string playerCurrentShape = playerShape.GetCurrentShapeName();

        if (incomingShape == playerCurrentShape)
        {
            score++;
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

    private void EndGame()
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
}