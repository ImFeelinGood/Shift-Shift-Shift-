using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour
{
    public GameObject[] shapes;
    public Transform spawnPoint;
    public Transform player;
    public float spawnInterval = 2f;
    public float moveSpeed = 5f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnShape), spawnInterval, spawnInterval);
    }

    void SpawnShape()
    {
        int randomIndex = Random.Range(0, shapes.Length);
        GameObject shape = Instantiate(shapes[randomIndex], spawnPoint.position, Quaternion.identity);

        ShapeMover mover = shape.AddComponent<ShapeMover>();
        mover.Initialize(player, moveSpeed, shapes[randomIndex].name);
    }
}