using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public PlayerShapes playerShapePrefab;
    public ShapeSpawner spawnerPrefab;
    public Transform spawnArea;
    public Transform cameraTarget;

    private int spawnCount = 0;
    private List<PlayerShapes> players = new List<PlayerShapes>();
    private HashSet<int> triggeredThresholds = new HashSet<int>();
    private List<int> spawnThresholds = new List<int> { 6, 16, 31, 51 };

    public void ResetGame()
    {
        players.Clear();
        spawnCount = 0;
        SpawnNewSpawnerAndPlayer(isFirstSpawn: true);
    }

    public bool CheckShapeMatch(string incomingShape)
    {
        PlayerShapes currentPlayer = players.FirstOrDefault(player => player.GetCurrentShapeName() == incomingShape);
        return currentPlayer != null;
    }

    private void SpawnNewSpawnerAndPlayer(bool isFirstSpawn = false)
    {
        Vector3 spawnerPosition = new Vector3(0, 0, 25);
        Vector3 playerPosition = new Vector3(0, 0, -5);

        if (!isFirstSpawn)
        {
            float xOffset = 0;
            switch (spawnCount)
            {
                case 1: xOffset = 2.5f; break;
                case 2: xOffset = -2.5f; break;
                case 3: xOffset = 5f; break;
                case 4: xOffset = -5f; break;
            }
            spawnerPosition = new Vector3(xOffset, 0, 25);
            playerPosition = new Vector3(xOffset, 0, -5);
        }

        ShapeSpawner newSpawner = Instantiate(spawnerPrefab, spawnerPosition, Quaternion.identity, spawnArea);
        PlayerShapes newPlayer = Instantiate(playerShapePrefab, playerPosition, Quaternion.identity, spawnArea);
        newSpawner.player = newPlayer.transform;
        players.Add(newPlayer);
        spawnCount++;
    }
}
