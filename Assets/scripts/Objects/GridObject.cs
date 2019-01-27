using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseObject))]
public class GridObject : MonoBehaviour, IStorable
{
    
    [HideInInspector]
    public GridCell previousCell;
    [HideInInspector]
    public GridCell currentCell;
    [HideInInspector]
    public GridCell nextCell;

    void Awake()
    {
        currentCell = GridCell.FromPosition(transform.position);
        previousCell = GridCell.FromPosition(transform.position);
        nextCell = GridCell.FromPosition(transform.position);
    }

    public bool isMoving { get { return currentCell != nextCell; }}

    public void MoveUp()
    {
        nextCell = currentCell.up;
    }

    public void MoveDown()
    {
        nextCell = currentCell.down;
    }

    public void MoveLeft()
    {
        nextCell = currentCell.left;
    }

    public void MoveRight()
    {
        nextCell = currentCell.right;
    }

    public List<float> GetData()
    {
        return new List<float>{(float)currentCell.x, (float)currentCell.y};
    }

    public void SetData(List<float> newData)
    {
        previousCell = currentCell;
        currentCell = new GridCell((int)newData[0], (int)newData[1]);
    }

    public void ApplyData(float t = 1.0f)
    {
        transform.position = new Vector3 (
            Mathf.Lerp((float)previousCell.x, (float)currentCell.x, t),
            transform.position.y,
            Mathf.Lerp((float)previousCell.y, (float)currentCell.y, t)
        );
    }

}
