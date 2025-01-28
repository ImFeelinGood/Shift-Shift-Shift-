using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeBehavior : MonoBehaviour
{
    public GameObject shapeObject;
    public GameObject inObject;

    // Store the shape name to check later for matching
    public string shapeName;

    public void Initialize(bool isPlayer)
    {
        shapeObject.SetActive(isPlayer);
        inObject.SetActive(!isPlayer);

        // If it's not the player's shape, set the shape name for matching
        if (!isPlayer)
        {
            shapeName = this.gameObject.name;  // Set the name for the incoming shape
        }
    }
}
