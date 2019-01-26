using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridObject))]
public class CollisionObject : MonoBehaviour
{
    public GridObject gridObject;

    public void Awake()
    {
        gridObject = GetComponent<GridObject>();
        GameManager.Instance.collisionObjects.Add(this);
    }
}
