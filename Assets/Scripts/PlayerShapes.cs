using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShapes : MonoBehaviour
{
    public GameObject[] shapes;
    private int currentShapeIndex = 0;
    private GameObject currentShape;

    void Start()
    {
        ChangeShape(0);
    }

    void ChangeShape(int newShapeIndex)
    {
        if (currentShape != null)
            Destroy(currentShape);

        currentShapeIndex = newShapeIndex;
        currentShape = Instantiate(shapes[currentShapeIndex], transform.position, Quaternion.identity, transform);
    }

    public string GetCurrentShapeName()
    {
        return shapes[currentShapeIndex].name;
    }

    private void OnMouseDown()
    {
        ChangeShape((currentShapeIndex + 1) % shapes.Length);
    }
}
