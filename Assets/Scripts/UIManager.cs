using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject startUI;
    public GameObject gameUI;
    public GameManager gameManager;

    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;
    public Button ultraModeButton;

    private void Start()
    {
        startUI.SetActive(true);
        gameUI.SetActive(false);

        easyButton.onClick.AddListener(() => SetDifficulty(Difficulty.Easy));
        mediumButton.onClick.AddListener(() => SetDifficulty(Difficulty.Medium));
        hardButton.onClick.AddListener(() => SetDifficulty(Difficulty.Hard));
        ultraModeButton.onClick.AddListener(() => SetDifficulty(Difficulty.UltraMode));

        easyButton.gameObject.SetActive(true);
        mediumButton.gameObject.SetActive(true);
        hardButton.gameObject.SetActive(true);
        ultraModeButton.gameObject.SetActive(true);
    }

    public void SetDifficulty(Difficulty difficulty)
    {
        gameManager.SetDifficulty(difficulty);

        startUI.SetActive(false);
        gameUI.SetActive(true);
        gameManager.StartGame();
    }
}
