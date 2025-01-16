using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeMover : MonoBehaviour
{
    private Transform target;
    private float speed;
    private string shapeName;

    public void Initialize(Transform target, float speed, string shapeName)
    {
        this.target = target;
        this.speed = speed;
        this.shapeName = shapeName;
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.position) < 0.5f)
            {
                GameManager.Instance.CheckShapeMatch(shapeName);
                Destroy(gameObject);
            }
        }
    }
}