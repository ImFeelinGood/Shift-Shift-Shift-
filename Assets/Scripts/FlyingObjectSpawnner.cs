using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingObjectSpawner : MonoBehaviour
{
    public GameObject flyingobjectPrefab; 
    public int flyingobjectCount = 5; 
    public float spawnRangeX = 5f; 
    public float spawnRangeY = 5f; 
    public float despawnZPosition = -20f; 
    public float minSpawnInterval = 1f; 
    public float maxSpawnInterval = 3f; 

    private List<GameObject> flyingobjects = new List<GameObject>(); 
    private int spawnedflyingobjectCount = 0; 

    private void Start()
    {
        StartCoroutine(SpawnflyingobjectsOneByOne());
    }

    private void Update()
    {
        DespawnOutOfBoundsflyingobjects();
    }

    private IEnumerator SpawnflyingobjectsOneByOne()
    {
        while (spawnedflyingobjectCount < flyingobjectCount)
        {
            Spawnflyingobject();
            spawnedflyingobjectCount++;

            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void Spawnflyingobject()
    {
        Vector3 position = new Vector3(
            transform.position.x + Random.Range(-spawnRangeX, spawnRangeX), 
            transform.position.y + Random.Range(-spawnRangeY, spawnRangeY), 
            transform.position.z 
        );

        GameObject flyingobject = Instantiate(flyingobjectPrefab, position, Quaternion.identity);
        flyingobject.transform.SetParent(transform); 
        flyingobjects.Add(flyingobject); 
    }

    private void DespawnOutOfBoundsflyingobjects()
    {
        for (int i = flyingobjects.Count - 1; i >= 0; i--)
        {
            if (flyingobjects[i] != null && flyingobjects[i].transform.position.z < despawnZPosition)
            {
                Destroy(flyingobjects[i]);
                flyingobjects.RemoveAt(i);
            }
        }
    }
}
