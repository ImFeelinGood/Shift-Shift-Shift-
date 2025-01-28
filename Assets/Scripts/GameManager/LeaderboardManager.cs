using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

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

public class LeaderboardManager : MonoBehaviour
{
    private const string LeaderboardKey = "LeaderboardData";
    private List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();

    public TextMeshProUGUI highScoreTextMenu;
    public GameObject leaderboardRowPrefab;
    public Transform leaderboardContent;

    public void LoadLeaderboard()
    {
        if (PlayerPrefs.HasKey(LeaderboardKey))
        {
            string json = PlayerPrefs.GetString(LeaderboardKey);
            SerializableLeaderboard savedLeaderboard = JsonUtility.FromJson<SerializableLeaderboard>(json);
            leaderboard = savedLeaderboard.ToList();
        }
    }

    public void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(new SerializableLeaderboard(leaderboard));
        PlayerPrefs.SetString(LeaderboardKey, json);
        PlayerPrefs.Save();
    }

    public void AddToLeaderboard(string playerName, int score, Difficulty difficulty)
    {
        leaderboard.Add(new LeaderboardEntry(playerName, score, difficulty));
        SaveLeaderboard();
    }

    public void DisplayLeaderboard()
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
}
