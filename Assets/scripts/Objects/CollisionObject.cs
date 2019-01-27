using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridObject))]
public class CollisionObject : MonoBehaviour
{

    [HideInInspector]
    public GridObject gridObject;

    public void Awake()
    {
        gridObject = GetComponent<GridObject>();
    }
}
