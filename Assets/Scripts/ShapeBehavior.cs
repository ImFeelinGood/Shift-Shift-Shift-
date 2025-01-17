using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeBehavior : MonoBehaviour
{
    public GameObject shapeObject;
    public GameObject inObject;

    public void Initialize(bool isPlayer)
    {
        shapeObject.SetActive(isPlayer);
        inObject.SetActive(!isPlayer);
    }
}