using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour
{
    public GameObject[] shapes;
    public Transform spawnPoint;
    public Transform player;
    public float minSpawnInterval = 6f;
    public float maxSpawnInterval = 10f;
    public float moveSpeed = 5f;

    private void Start()
    {
        SetNextSpawn();
    }

    void SetNextSpawn()
    {
        float randomInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
        Invoke(nameof(SpawnShape), randomInterval);
    }

    void SpawnShape()
    {
        int randomIndex = Random.Range(0, shapes.Length);
        GameObject shape = Instantiate(shapes[randomIndex], spawnPoint.position, Quaternion.identity);

        ShapeMover mover = shape.AddComponent<ShapeMover>();
        mover.Initialize(player, moveSpeed, shapes[randomIndex].name);

        SetNextSpawn();
    }
}
