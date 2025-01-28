using UnityEngine;

public enum Difficulty
{
    Easy,
    Medium,
    Hard,
    UltraMode
}

public class DifficultyManager : MonoBehaviour
{
    public Difficulty CurrentDifficulty { get; private set; }

    public float minSpawnInterval = 8f;
    public float maxSpawnInterval = 10f;
    public float moveSpeed = 4f;

    public void SetDifficulty(Difficulty difficulty)
    {
        CurrentDifficulty = difficulty;
        AdjustDifficultySettings();
    }

    private void AdjustDifficultySettings()
    {
        switch (CurrentDifficulty)
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
    }
}

