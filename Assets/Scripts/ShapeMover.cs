using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeMover : MonoBehaviour
{
    private Transform target;
    private float speed;
    private string shapeName;
    private Vector3 direction;
    private Vector3 startPosition;
    private float destroyDistance = 45f;

    public void Initialize(Transform target, float speed, string shapeName)
    {
        this.target = target;
        this.speed = speed;
        this.shapeName = shapeName;

        startPosition = transform.position;
        if (target != null)
        {
            direction = (target.position - transform.position).normalized;
        }
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position += direction * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, target.position) < 0.5f)
            {
                PlayerShapes player = target.GetComponent<PlayerShapes>();

                if (player != null && player.GetCurrentShapeName() == shapeName)
                {
                    GameManager.Instance.CheckShapeMatch(shapeName);
                }
                else
                {
                    Debug.Log("Game Over! Boohoo... Wrong Shapeeee!!!.");
                    GameManager.Instance.EndGame();
                }

                target = null;
            }
        }
        else
        {
            transform.position += direction * speed * Time.deltaTime;
        }

        if (Vector3.Distance(transform.position, startPosition) > destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}
