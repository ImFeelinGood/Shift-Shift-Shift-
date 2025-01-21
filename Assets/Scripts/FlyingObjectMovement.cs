using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingObjectMovement : MonoBehaviour
{
    public float speedMin = 0.8f;
    public float speedMax = 1.2f;
    public float resetPositionZ = -20f; 
    public float startPositionZ = 20f;
    private float flyingobjectSpeed;

    private void Start()
    {
        flyingobjectSpeed = Random.Range(speedMin, speedMax);
    }

    private void Update()
    {
        transform.Translate(Vector3.back * flyingobjectSpeed * Time.deltaTime);

        if (transform.position.z < resetPositionZ)
        {
            ResetCloudPosition();
        }
    }

    private void ResetCloudPosition()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, startPositionZ);
    }
}
